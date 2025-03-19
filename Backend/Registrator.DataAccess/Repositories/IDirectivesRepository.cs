using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Registrator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registrator.DataAccess.Repositories
{
    public interface IDirectivesRepository : IRepository<Directive>
    {
        Task<Guid> Create(byte[] content, string fileName);
    }
}
