using Ideal.Core.Common.Extensions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Ideal.Core.Common.Helpers
{
    /// <summary>
    /// 邮件帮助类
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="fromName">发送人姓名</param>
        /// <param name="fromAddress">发送人邮箱</param>
        /// <param name="tos">接收人姓名：发送人邮箱</param>
        /// <param name="text">邮件内容支持html</param>
        /// <param name="textFormat"></param>
        /// <param name="option">邮件内容支持html</param>
        /// <returns></returns>
        public static string Send(string subject, string fromName, string fromAddress, Dictionary<string, string> tos, string text, TextFormatEnum textFormat, EmailOption option)
        {
            using var smtp = new SmtpClient();
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(fromName, fromAddress));
            foreach (var to in tos)
            {
                mail.To.Add(new MailboxAddress(to.Key, to.Value));
            }

            mail.Subject = subject;
            if (string.IsNullOrEmpty(text))
            {
                return "发送内容不能为空";
            }

            var Html = new TextPart(TextFormat.Text)
            {
                Text = text,
            };

            mail.Body = new Multipart("mixed")
                {
                    Html
            };

            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            //连接邮箱服务器 
            smtp.Connect(option.Server, option.Port, SecureSocketOptions.None);
            //登录认证 邮箱账号和授权密钥
            smtp.Authenticate(option.User, option.Password);
            smtp.Timeout = 600000;
            string res = smtp.Send(mail);
            smtp.Disconnect(true);
            return res;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmailOption
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public string Server { get; set; } = null!;

        /// <summary>
        /// 监听端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ProtocolTypeEnum
    {
        /// <summary>
        /// Imap
        /// </summary>
        Imap = 1,

        /// <summary>
        /// Pop3
        /// </summary>
        Pop3 = 2,

        /// <summary>
        /// Smtp
        /// </summary>
        Smtp = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TextFormatEnum
    {
        /// <summary>
        /// The plain text format.
        /// </summary>
        Plain = 0,

        /// <summary>
        /// An alias for the plain text format.
        /// </summary>
        Text = 0,

        /// <summary>
        /// The flowed text format (as described in rfc3676).
        /// </summary>
        Flowed = 1,

        /// <summary>
        /// The HTML text format.
        /// </summary>
        Html = 2,

        /// <summary>
        /// The enriched text format.
        /// </summary>
        Enriched = 3,

        /// <summary>
        /// The compressed rich text format.
        /// </summary>
        CompressedRichText = 4,

        /// <summary>
        /// The rich text format.
        /// </summary>
        RichText = 5
    }
}
