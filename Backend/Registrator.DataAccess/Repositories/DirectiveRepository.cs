using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Registrator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registrator.DataAccess.Repositories
{
    public class DirectiveRepository : EFRepository<Directive, RegistratorDbContext>,
        IDirectivesRepository
    {
        private readonly RegistratorDbContext context;
        public DirectiveRepository(RegistratorDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<Guid> Create(byte[] content, string fileName)
        {
            var directive = Directive.Create(content, fileName);

            await AddAsync(directive, default);

            await context.SaveChangesAsync();

            return directive.Id;
        }
    }
}
