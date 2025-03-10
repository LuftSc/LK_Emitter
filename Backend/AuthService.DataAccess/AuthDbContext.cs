using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.DataAccess
{
    public class AuthDbContext : DbContext, IUnitOfWork
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Помимо базово функционала SaveChangesAsync
            // нам нужно проработать историю, связанную с Mediator'ом,
            // который бы обрабатывал наши Event'ы

            // Если Id-шники на месте (у объектов),
            // то тогда вообще никаких вопросов не возникает, всё ОК
            // Можно вызывать наш DispatchDomainEventsAsync двумя способами

            // внутри EF у нас есть возможность обратиться к ServiceProvider'у
            // в текущем scope, в котором он создан
            //var mediator = this.GetService<IMediator>();
            //await this.DispatchDomainEventsAsync(mediator);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    /*public static class ServerDbContextExtensions
    {
        public static async Task DispatchDomainEventsAsync(this ServerDbContext dbContext, IMediator mediator)
        {// Обрабатывать все наши Event'ы мы будем через Mediator, ведь по факту
         // событие распространяется не точка - в - точку (1 событие - 1 хэндлер)
         // А событий может быть МНОЖЕСТВО ~ создался customer => Что будет происходить, когда создался Customer?
         // Мы будем оповещать систему, что мы его создали

            // Получаем все доменные сущности (Entity), которые мы в рамках нашего context'a обрабатываем
            // Мы получаем их из ChangeTracker'a,
            // который (под капотом) сейчас работает на DbContext'e
            var domainEntities = dbContext.ChangeTracker.Entries<Entity>()
                .Where(x => x.Entity.DomainEvents?.Any() ?? false) // Нам интересны те сущности, у которых есть Доменные События
                .ToList(); // Мы получаем список сущностей, у которых есть Доменные события

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents) // Итерируемся по этим данным
                .ToList(); // Получаем список доменных событий из списка сущностей, у которых есть Доменные события

            // Это нужно для того, чтобы не повторить те же самые события, которые у нас там существуют
            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents()); // Очищаем список доменных событий

            // После того, как мы получили все события и удалили их из всех существующих событий
            // Мы должны их как-то вызвать
            // Мы подготовили асинхронный процесс, вызвав несколько хэндлеров(в рамках медиатора - уведомления)
            // И с ними мы теперь сможем обработать всё, что нам необходимо
            var tasks = domainEvents.Select(async domainEvent => await mediator.Publish(domainEvent, CancellationToken.None));

            // Мы обязаны дождаться выполнения, потому что Task'и могут быть разными в рамках этого процесса
            await Task.WhenAll(tasks);
        }
    } */
}
