using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres
{
    public class Document : Entity<Guid>, IAggregateRoot
    {
        private Document() : base(Guid.NewGuid())
        {
        }
        private Document(
            User user,
            string fileName,
            DateTime uploadDate,
            byte[] content,
            string hash
            ) : base(Guid.NewGuid())
        {
            User = user;
            Console.WriteLine(user.Registrator);
            IsEmitterSended = user.Registrator is not null;
            Title = Path.GetFileNameWithoutExtension(fileName);
            Type = Path.GetExtension(fileName).ToLowerInvariant();
            UploadDate = uploadDate;
            Content = content;
            Hash = hash;
        }
        public User User { get; private set; } = null!;
        public bool IsEmitterSended { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;
        public DateTime UploadDate { get; private set; }
        public byte[] Content { get; private set; } = [];
        public string Hash { get; private set; } = string.Empty;
        public double GetSize() => Content.Length / 1024;
        public Emitter Emitter { get; private set; } = null!;
        public static Result<Document> Create(User user,
            string fileName,
            DateTime uploadDate,
            byte[] content,
            string hash)
        {
            var document = new Document(user, fileName, uploadDate, content, hash);
            
            return Result<Document>.Success(document);
        }
    }
}
