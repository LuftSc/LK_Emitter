using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Storage
{
    public interface IRepository<TAggregateRoot> : IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        ValueTask<TAggregateRoot> AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);

        // Использую IReadOnlyList, а не ReadOnlyList,
        // Потому что ReadOnlyList можно скастовать в обычный лист и его можно будет изменять
        Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken);
        Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        Task RemoveRangeAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        IUnitOfWork UnitOfWork { get; }
    }
}
