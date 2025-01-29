using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Storage
{
    public abstract class Repository<TAggregateRoot>(IUnitOfWork unitOfWork)
        : ReadOnlyRepository<TAggregateRoot>,
        IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (ReadOnly)
                {
                    throw new NotSupportedException("For current repository enable readonly in read-only");
                }
                return unitOfWork;
            }
        }
        public abstract ValueTask<TAggregateRoot> AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        public abstract Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, CancellationToken cancellationToken);
        public abstract Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        public abstract Task RemoveRangeAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken);
        
    }
}
