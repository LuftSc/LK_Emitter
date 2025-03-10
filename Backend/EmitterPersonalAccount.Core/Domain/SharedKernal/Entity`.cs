using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal
{
    public abstract class Entity<TKey> : Entity
    {
        protected Entity(TKey id)
        {
            Id = id;
        }
        public TKey Id { get; private set; } 
    }
}
