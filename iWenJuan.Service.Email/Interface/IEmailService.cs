namespace iWenJuan.Service.Email.Interface;

/// <summary>
/// 异步发送电子邮件的服务接口。
/// </summary>
public interface IEmailService
{
	/// <summary>
	/// 异步发送电子邮件。
	/// </summary>
	/// <param name="toEmail">收件人电子邮件地址。</param>
	/// <param name="subject">电子邮件主题。</param>
	/// <param name="body">电子邮件正文内容。</param>
	/// <returns>表示异步操作的任务。</returns>
	Task SendEmailAsync(string toEmail, string subject, string body);
}
