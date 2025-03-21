using iWenJuan.Service.Auth.Interface;
using RabbitMQ.Client;
using System.Text;

namespace iWenJuan.Service.Auth.Services;

/// <summary>
/// RabbitMQService 实现了 IMessageQueueService 接口，用于发送消息到 RabbitMQ 队列。
/// </summary>
public class RabbitMQService : IMessageQueueService
{
	private readonly IConnection _connection;

	/// <summary>
	/// 初始化 RabbitMQService 类的新实例。
	/// </summary>
	/// <param name="connection">RabbitMQ 连接对象。</param>
	public RabbitMQService(IConnection connection)
	{
		_connection = connection;
	}

	/// <summary>
	/// 发送消息到指定的 RabbitMQ 队列。
	/// </summary>
	/// <param name="queueName">队列名称。</param>
	/// <param name="message">要发送的消息。</param>
	public void SendMessage(string queueName, string message)
	{
		// 使用连接对象创建一个新的通道
		using (var channel = _connection.CreateModel())
		{
			// 声明一个队列，如果队列不存在则创建
			channel.QueueDeclare(queue: queueName,
								 durable: true, // 设置队列是否持久化
								 exclusive: false, // 设置队列是否为排他队列
								 autoDelete: false, // 设置队列是否自动删除
								 arguments: null); // 其他参数

			// 将消息内容转换为字节数组
			var body = Encoding.UTF8.GetBytes(message);

			// 发布消息到指定的队列
			channel.BasicPublish(exchange: "", // 使用默认交换机
								 routingKey: queueName, // 路由键为队列名称
								 basicProperties: null, // 消息属性
								 body: body); // 消息内容
		}
	}

	/// <summary>
	/// 发送消息到固定的 RabbitMQ 队列。
	/// </summary>
	/// <param name="message">要发送的消息。</param>
	public void SendMessage(string message)
	{
		SendMessage("email_verification_queue", message);
	}
}
