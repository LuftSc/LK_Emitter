//using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.LocationVO;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres
{
    public class Registrator : Entity<Guid>, IAggregateRoot
    {
        private Registrator() : base(Guid.NewGuid()) { }
        private Registrator(OGRNInfo ogrn, Location location)
            : base(Guid.NewGuid())
        {
            OGRN = ogrn;
            Location = location;
        }
        public OGRNInfo OGRN { get; private set; }
        public Location Location { get; private set; }
        //public List<User> Users { get; private set; } = [];
        //public List<Emitter> Emitters { get; private set; } = [];
        public static Result<Registrator> Create(OGRNInfo ogrn, Location location)
        {
            return Result<Registrator>.Success(new Registrator(ogrn, location));
        }
    }
}
