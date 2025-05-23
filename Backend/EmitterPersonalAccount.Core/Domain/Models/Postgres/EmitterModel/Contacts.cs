using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel
{
    public class Contacts : ValueObject, IXmlSerializable
    {
        private Contacts()
        {
            
        }
        private Contacts(string phoneNumber, string fax,
            string email, int okopf)
        {
            PhoneNumber = phoneNumber;
            Fax = fax;
            Email = email;
            OKOPF = okopf;
        }

        public static readonly Contacts Empty = new ();

        [XmlElement]
        public string PhoneNumber { get; private set; }

        [XmlElement]
        public string Fax { get; private set; }

        [XmlElement]
        public string Email { get; private set; }

        [XmlElement]
        public int OKOPF { get; private set; }
        public static Result<Contacts> Create(string phoneNumber, string fax,
            string email, int okopf)
        {
            return Result.Success(new Contacts(phoneNumber, fax, email, okopf));    
        }

        // Сериализация в XML
        public string ToXml()
        {
            var serializer = new XmlSerializer(typeof(Contacts));
            using var writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.ToString();
        }

        // Десериализация из XML
        public static Contacts FromXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(Contacts));
            using var reader = new StringReader(xml);
            return (Contacts)serializer.Deserialize(reader);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PhoneNumber;
            yield return Fax;
            yield return Email;
            yield return OKOPF;
        }

        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            PhoneNumber = reader.ReadElementContentAsString(nameof(PhoneNumber), "");
            Fax = reader.ReadElementContentAsString(nameof(Fax), "");
            Email = reader.ReadElementContentAsString(nameof(Email), "");
            OKOPF = reader.ReadElementContentAsInt(nameof(OKOPF), "");
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(PhoneNumber), PhoneNumber);
            writer.WriteElementString(nameof(Fax), Fax);
            writer.WriteElementString(nameof(Email), Email);
            writer.WriteElementString(nameof(OKOPF), OKOPF.ToString());
        }
    }
}
