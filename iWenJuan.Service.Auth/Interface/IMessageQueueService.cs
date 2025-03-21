namespace iWenJuan.Service.Auth.Interface;

/// <summary>
/// 发送消息到指定队列的接口。
/// </summary>
public interface IMessageQueueService
{
	/// <summary>
	/// 发送消息到指定队列。
	/// </summary>
	/// <param name="queueName">队列名称。</param>
	/// <param name="message">要发送的消息。</param>
	void SendMessage(string queueName, string message);

	/// <summary>
	/// 发送消息到固定队列。
	/// queueName = email_verification_queue
	/// </summary>
	/// <param name="message">要发送的消息。</param>
	void SendMessage(string message);
}
