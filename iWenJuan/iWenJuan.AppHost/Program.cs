using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

#region appsettings.json 配置
//// 配置 JWT 参数（会通过配置传递给服务）
//var jwtSettings = builder.AddParameter("JwtSecret",
//	builder.Configuration.GetSection("Jwt"));
//// 配置 Email 参数（会通过配置传递给服务）
//var emailSettings = builder.AddParameter("EmailSettings",
//	builder.Configuration["Email"] ?? throw new InvalidOperationException("EmailSettings is not configured."));
//// 配置 Endpoint 参数（会通过配置传递给服务）
//var endpointSettings = builder.AddParameter("EndpointSettings",
//	builder.Configuration["Endpoint"] ?? throw new InvalidOperationException("AuthService endpoint is not configured."));
#endregion appsettings.json 配置

#region 中间件配置
// 消息队列
var mq = builder.AddRabbitMQ("mq");
// 缓存
var redis = builder.AddRedis("redis");
#endregion 中间件配置

#region 数据库配置
// 数据库
var pgSql = builder.AddPostgres("PostgreSQL")
	.WithDataVolume("iWenJuanDB")
	.WithPgAdmin();
// 用户数据库
var usrDb = pgSql.AddDatabase("usrDb");
// 问卷数据库
var surveyDataDb = pgSql.AddDatabase("surveyDataDb");
// 数据库-问卷数据存储
var surveyDataStorageDb = pgSql.AddDatabase("surveyDataStorageDb");
#endregion 数据库配置

#region 微服务配置
// 服务-邮件服务
var emailService = builder.AddProject<Projects.iWenJuan_Service_Email>("iwenjuan-service-email")
	.WithReference(mq);

// 服务-用户认证服务
var authService = builder.AddProject<Projects.iWenJuan_Service_Auth>("iwenjuan-service-auth")
	.WithReference(usrDb)
	.WithReference(mq)
	.WithReference(redis)
	.WithReference(emailService);

// 服务-问卷调查服务
var surveyService = builder.AddProject<Projects.iWenJuan_Service_Survey>("iwenjuan-service-survey")
	.WithReference(surveyDataDb);

// 服务-问卷数据存储服务
var dataStorage = builder.AddProject<Projects.iWenJuan_Service_DataStorage>("iwenjuan-service-datastorage")
	.WithReference(surveyDataStorageDb)
	.WithReference(surveyService);

// 服务器-问卷数据处理微服务
var dataProcessing = builder.AddProject<Projects.iWenJuan_Service_DataProcessing>("iwenjuan-service-dataprocessing")
	.WithReference(dataStorage);

// 服务器-问卷模板社区微服务
var templateCommunity = builder.AddProject<Projects.iWenJuan_Service_TemplateCommunity>("iwenjuan-service-templatecommunity")
	.WithReference(surveyDataDb);

// 前端-问卷编辑工作台
builder.AddProject<Projects.iWenJuan_Client_WorkSpace>("iwenjuan-client-workspace")
	.WithReference(authService)
	.WithReference(surveyService)
	.WithReference(dataStorage)
	.WithReference(dataProcessing)
	.WithReference(templateCommunity);

// 前端-问卷调查微服务
builder.AddProject<Projects.iWenJuan_Client_Answer>("iwenjuan-client-answer")
	.WithReference(surveyService);
#endregion 微服务配置

builder.Build().Run();
