using iWenJuan.Service.Email.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace iWenJuan.Service.Email.Services;

/// <summary>
/// 提供电子邮件服务的实现类。
/// </summary>
public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;
	private readonly ILogger<EmailService> _logger;

	/// <summary>
	/// 初始化 <see cref="EmailService"/> 类的新实例。
	/// </summary>
	/// <param name="configuration">配置对象。</param>
	public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	/// <summary>
	/// 异步发送电子邮件。
	/// </summary>
	/// <param name="toEmail">收件人电子邮件地址。</param>
	/// <param name="subject">电子邮件主题。</param>
	/// <param name="body">电子邮件正文。</param>
	/// <returns>表示异步操作的任务。</returns>
	public async Task SendEmailAsync(string toEmail, string subject, string body)
	{
		//// 创建邮件对象
		//MailMessage mail = new MailMessage(_configuration["Email:Smtp:From"]!, toEmail);
		//mail.Subject = subject;
		//mail.Body = body;

		//// 创建SMTP客户端
		//SmtpClient smtpClient = new SmtpClient(_configuration["Email:Smtp:Host"]);
		//smtpClient.Port = int.Parse(_configuration["Email:Smtp:Port"]!);
		//smtpClient.Credentials = new NetworkCredential(_configuration["Email:Smtp:Username"], _configuration["Email:Smtp:Password"]);
		//smtpClient.EnableSsl = true;

		//// 发送邮件
		//smtpClient.Send(mail);


		// 创建一个新的电子邮件消息
		var emailMessage = new MimeMessage();
		// 设置发件人地址
		emailMessage.From.Add(
			new MailboxAddress(_configuration["Email:Smtp:From"], _configuration["Email:Smtp:Username"]));
		// 设置收件人地址
		emailMessage.To.Add(
			new MailboxAddress("", toEmail));
		// 设置邮件主题
		emailMessage.Subject = subject;
		// 设置邮件正文
		emailMessage.Body = new TextPart("plain") { Text = body };

		// 使用SmtpClient发送邮件
		using (var client = new SmtpClient())
		{
			// 连接到SMTP服务器
			await client.ConnectAsync(
				_configuration["Email:Smtp:Host"], int.Parse(_configuration["Email:Smtp:Port"]));
			// 使用用户名和密码进行身份验证
			await client.AuthenticateAsync(
				_configuration["Email:Smtp:Username"], _configuration["Email:Smtp:Password"]);
			// 发送邮件
			await client.SendAsync(emailMessage);
			// 断开与SMTP服务器的连接
			await client.DisconnectAsync(true);
		}

		// 记录日志
		_logger.LogInformation($"发送邮件到{toEmail}:\t主题{subject}\t|\t内容{body}", toEmail, subject, body);
	}
}
