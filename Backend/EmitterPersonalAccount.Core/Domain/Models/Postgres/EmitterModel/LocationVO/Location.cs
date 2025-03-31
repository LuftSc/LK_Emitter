using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.LocationVO
{
    [Owned]
    public class Location : ValueObject
    {
        private Location()
        {
            
        }
        private Location(string country, int index, 
            string region, Address address)
        {
            Country = country;
            Index = index;
            Region = region;
            Address = address;
        }
        public string Country { get; private set; }
        public int Index { get; private set; } = 0;
        public string Region { get; private set; }
        public Address Address { get; private set; }
        public static Result<Location> Create(string country, int index,
            string region, Address address)
        {
            return Result.Success(new Location(country, index, region, address));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return Index;
            yield return Region;
            yield return Address;
        }
    }
}
