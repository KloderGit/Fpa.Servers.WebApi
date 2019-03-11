using Common.Extensions.ContactDomain;
using Domain.Models.Crm;
using LibraryAmoCRM.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiLogic.Logics.SignUp.Fabric
{
    public class ContactBuilder
    {
        Contact contact;

        public ContactBuilder()
        {
            contact = new Contact();
        }

        public ContactBuilder(Contact contact)
        {
            this.contact = contact;
        }

        public ContactBuilder Name(string value)
        {
            if (String.IsNullOrEmpty( value )) value = "Имя не указано";

            this.contact.Name = value; ;

            return this;
        }

        public ContactBuilder Phone(string value)
        {
            if (String.IsNullOrEmpty( value )) return this;

            this.contact.Phones( PhoneTypeEnum.MOB, value );
            this.contact.Fields = this.contact.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.contact.Fields = this.contact.GetChanges().Fields;

            return this;
        }

        public ContactBuilder Email(string value)
        {
            if (String.IsNullOrEmpty( value )) return this;

            this.contact.Email( EmailTypeEnum.PRIV, value );
            this.contact.Fields = this.contact.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.contact.Fields = this.contact.GetChanges().Fields;

            return this;
        }

        public ContactBuilder City(string value)
        {
            if (String.IsNullOrEmpty( value )) return this;

            this.contact.City( value );
            this.contact.Fields = this.contact.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.contact.Fields = this.contact.GetChanges().Fields;

            return this;
        }

        public Contact Get()
        {
            return this.contact;
        }

        public static implicit operator Contact(ContactBuilder builder)
        {
            return builder.Get();
        }
    }
}
