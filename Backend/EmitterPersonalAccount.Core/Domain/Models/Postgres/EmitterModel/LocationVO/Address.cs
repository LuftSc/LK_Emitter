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
    public class Address : ValueObject
    {
        private Address()
        {
            
        }
        private Address(string city, string street, string homeNumber)
        {
            City = city;
            Street = street;
            HomeNumber = homeNumber;
        }
        public string City { get; private set; }
        public string Street { get; private set; }
        public string HomeNumber { get; private set; }
        public static Result<Address> Create(string city, string street, string homeNumber)
        {
            return Result.Success(new Address(city, street, homeNumber));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return Street;
            yield return HomeNumber;
        }

        // Метод для преобразования в строку (например, JSON или разделитель)
        public override string ToString()
            => $"{City}|{Street}|{HomeNumber}";

        // Метод для парсинга строки обратно в объект
        public static Address Parse(string value)
        {
            var parts = value.Split('|');
            return new Address(parts[0], parts[1], parts[2]);
        }
    }
}
