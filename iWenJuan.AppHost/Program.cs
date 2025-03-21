var builder = DistributedApplication.CreateBuilder(args);

// 数据库
var pgsql = builder.AddPostgres("PostgreSQL")
	.WithDataVolume("PGSQL");
// 用户库
var userDb = pgsql.AddDatabase("UserDb");
// 数据库
var dataDb = pgsql.AddDatabase("DataDb");

// 缓存
var redis = builder.AddRedis("redis");
// 消息队列
var mq = builder.AddRabbitMQ("RabbitMq");

// 前端-客户端工作台
builder.AddProject<Projects.iWenJuan_Client_WorkSpace>("iwenjuan-client-workspace")
	.WithReference(userDb)
	.WithReference(redis);
// 服务器-数据处理微服务
builder.AddProject<Projects.iWenJuan_Server_DataProcessing>("iwenjuan-server-dataprocessing")
	.WithReference(dataDb)
	.WithReference(mq);

builder.Build().Run();
