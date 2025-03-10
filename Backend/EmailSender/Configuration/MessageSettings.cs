using MailKit.Security;

namespace EmailSender.Configuration
{
    public class MessageSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SmtpEmailAddress { get; set; }
        public string SmtpPassword { get; set; }
        public SecureSocketOptions SecureSocketOption { get; set; }
    }
}
