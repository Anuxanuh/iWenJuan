var builder = DistributedApplication.CreateBuilder(args);

// ���ݿ�
var pgsql = builder.AddPostgres("PostgreSQL")
	.WithDataVolume("PGSQL");
// �û���
var userDb = pgsql.AddDatabase("UserDb");
// ���ݿ�
var dataDb = pgsql.AddDatabase("DataDb");

// ����
var redis = builder.AddRedis("redis");
// ��Ϣ����
var mq = builder.AddRabbitMQ("RabbitMq");

// ǰ��-�ͻ��˹���̨
builder.AddProject<Projects.iWenJuan_Client_WorkSpace>("iwenjuan-client-workspace")
	.WithReference(userDb)
	.WithReference(redis);
// ������-���ݴ���΢����
builder.AddProject<Projects.iWenJuan_Server_DataProcessing>("iwenjuan-server-dataprocessing")
	.WithReference(dataDb)
	.WithReference(mq);

builder.Build().Run();
