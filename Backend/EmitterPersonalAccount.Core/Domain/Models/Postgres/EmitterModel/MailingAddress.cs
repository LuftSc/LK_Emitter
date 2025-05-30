﻿using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.LocationVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel
{
    public class MailingAddress : ValueObject
    {
        private MailingAddress()
        {
            
        }
        private MailingAddress(Location location, 
            Contacts contacts)
        {
            Location = location;
            Contacts = contacts;
        }

        public static readonly MailingAddress Empty = new();
        public Location Location { get; private set; } = Location.Empty;
        public Contacts Contacts { get; private set; } = Contacts.Empty;
        
        public static Result<MailingAddress> Create(Location location,
            Contacts contacts)
        {
            return Result.Success(new MailingAddress(location, contacts));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Location;
            yield return Contacts;
        }
    }
}
