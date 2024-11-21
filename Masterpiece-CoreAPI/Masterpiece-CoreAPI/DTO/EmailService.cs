using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("SMTP");
        var fromEmail = smtpSettings["Email"];
        var password = smtpSettings["Password"];
        var host = smtpSettings["Host"];
        var port = int.Parse(smtpSettings["Port"]);
        var enableSSL = bool.Parse(smtpSettings["EnableSSL"]);

        var smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = enableSSL
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true // لجعل محتوى البريد HTML
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
