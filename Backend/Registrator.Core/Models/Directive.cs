using EmitterPersonalAccount.Core.Domain.SharedKernal;

namespace Registrator.Core.Models
{
    public class Directive : Entity<Guid>, IAggregateRoot
    {
        public string MIMEType { get; private set; } = string.Empty;
        public string FileName { get; private set; } = string.Empty;
        public byte[] Content { get; private set; } = [];

        private Directive() : base(Guid.NewGuid()) { }
        private Directive(byte[] content, string fileName)
            : base(Guid.NewGuid())
        {
            MIMEType = "application/pdf";
            FileName = fileName;
            Content = content;
        }
        public static Directive Create(byte[] content, string fileName)
        {
            return new Directive(content, fileName);
        }
    }
}
