using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Storage
{
    public interface IUnitOfWork : IDisposable
    {
        // Task используем для получения статуса сохранения.
        // Успешно - ок, нет - кинет exception
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
