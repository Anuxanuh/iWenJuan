using iWenJuan.Service.Email.Interface;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using iWenJuan.Shared.Dtos;

namespace iWenJuan.Service.Email.Services;

/// <summary>
/// RabbitMQ消费者服务，用于处理从队列中接收的消息并发送电子邮件。
/// </summary>
public class RabbitMQConsumerService : BackgroundService
{
	private readonly IConnection _connection;
	private readonly IEmailService _emailService;
	private readonly ILogger<RabbitMQConsumerService> _logger;

	/// <summary>
	/// 初始化RabbitMQConsumerService的新实例。
	/// </summary>
	/// <param name="connection">RabbitMQ连接。</param>
	/// <param name="emailService">电子邮件服务。</param>
	public RabbitMQConsumerService(IConnection connection, IEmailService emailService, ILogger<RabbitMQConsumerService> logger)
	{
		_connection = connection;
		_emailService = emailService;
		_logger = logger;
		_logger.LogInformation("RabbitMQ消费者服务已初始化");
	}

	/// <summary>
	/// 执行异步任务，监听队列并处理接收到的消息。
	/// </summary>
	/// <param name="stoppingToken">用于取消操作的令牌。</param>
	/// <returns>表示异步操作的任务。</returns>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// 创建一个新的通道
		using (var channel = _connection.CreateModel())
		{
			// 声明一个队列，如果队列不存在则创建
			channel.QueueDeclare(queue: "email_verification_queue",
								 durable: true,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);

			// 创建一个事件消费者
			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (model, ea) =>
			{
				// 获取消息的主体并将其转换为字符串
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);

				// 反序列化消息为VerificationMessage对象
				var data = JsonSerializer.Deserialize<EmailVerificationMessage>(message);

				// 使用IEmailService发送电子邮件
				_emailService.SendEmailAsync(data!.Email, "iWenJuan邮箱验证码", $"你的验证码是: {data.Code}");

				// 记录日志
				_logger.LogInformation($"已发送验证码到{data.Email}，验证码为{data.Code}", data.Email, data.Code);
			};

			// 消费队列中的消息
			channel.BasicConsume(queue: "email_verification_queue",
								 autoAck: true,
								 consumer: consumer);

			// 完成任务
			await Task.CompletedTask;
		}

		// 记录日志
		_logger.LogInformation("RabbitMQ消费者服务已启动");
	}
}
