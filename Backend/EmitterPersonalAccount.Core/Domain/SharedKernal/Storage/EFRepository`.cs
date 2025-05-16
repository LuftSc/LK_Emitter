using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Storage
{
    public abstract class EFRepository<TAggregateRoot, TDbContext> : Repository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
        where TDbContext : DbContext, IUnitOfWork
    {
        private readonly DbContext _context;
        public EFRepository(TDbContext context) : base(context)
        {
            _context = context;
        }

        private DbSet<TAggregateRoot> _items => _context.Set<TAggregateRoot>();

        // Чтобы каким-то образом обращаться и расширять текущие наборы данных
        protected virtual IQueryable<TAggregateRoot> Items => ReadOnly ? _items.AsNoTracking() : _items;

        public override bool ReadOnly { get => base.ReadOnly; set => base.ReadOnly = value; }

        public override async ValueTask<TAggregateRoot> AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            var entry = await _context.AddAsync(aggregateRoot, cancellationToken);
            return entry.Entity;
        }

        public override Task AddRangeAsync(IReadOnlyList<TAggregateRoot> aggregateRoots, 
            CancellationToken cancellationToken)
        {
            return _items.AddRangeAsync(aggregateRoots, cancellationToken);
        }

        public override async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await Items.CountAsync(cancellationToken);
        }

        public override async Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Items.CountAsync(predicate, cancellationToken);
        }

        public override ValueTask<TAggregateRoot?> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {// Этот метод у EF кэширует данные у себя в Change трекере
            return _items.FindAsync(keyValues, cancellationToken);
        }

        public override async Task<TAggregateRoot> FirstAsync(CancellationToken cancellationToken)
        {
            return await Items.FirstAsync(cancellationToken);
        }

        public override async Task<TAggregateRoot> FirstAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Items.FirstAsync(predicate, cancellationToken);
        }

        public override async Task<TAggregateRoot?> FirstOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await Items.FirstOrDefaultAsync(cancellationToken);
        }

        public override async Task<TAggregateRoot?> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Items.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(CancellationToken cancellationToken)
        {
            return await Items.ToListAsync(cancellationToken);
        }

        public override async Task<IReadOnlyList<TAggregateRoot>> ListAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Items.Where(predicate).ToListAsync(cancellationToken);
        }

        public override async Task<long> LongCountAsync(CancellationToken cancellationToken)
        {
            return await Items.LongCountAsync(cancellationToken);
        }

        public override async Task<long> LongCountAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Items.LongCountAsync(predicate, cancellationToken);
        }

        public override async Task<IReadOnlyList<TResult>> QueryAsync<TResult>(Func<IQueryable<TAggregateRoot>, IQueryable<TResult>> predicate, CancellationToken cancellationToken)
        {
            return await predicate(Items).ToListAsync(cancellationToken);
        }

        public override Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            _items.Remove(aggregateRoot);

            return Task.CompletedTask;
        }

        public override Task RemoveRangeAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            _items.RemoveRange(aggregateRoot);

            return Task.CompletedTask;
        }

        public override async Task<TAggregateRoot> SingleAsync(CancellationToken cancellationToken)
        {
            return await Items.SingleAsync(cancellationToken);
        }

        public override async Task<TAggregateRoot> SingleAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Items.SingleAsync(predicate, cancellationToken);
        }

        public override async Task<TAggregateRoot?> SingleOrDefaultAsync(CancellationToken cancellationToken)
        {
            return await Items.SingleOrDefaultAsync(cancellationToken);
        }

        public override async Task<TAggregateRoot?> SingleOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken)
        {
            return await Items.SingleOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
