using Common.DTO.Service1C;
using Common.Extensions;
using Common.Extensions.ContactDomain;
using Domain.Models.Crm;
using Library1C;
using Library1C.DTO;
using Mapster;
using Microsoft.Extensions.Logging;
using ServiceReference1C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLogic.Infrastructure.Actions
{
    public class User1C
    {
        ILoggerFactory loggerFactory;
        ILogger currentLogger;

        UnitOfWork database;
        TypeAdapterConfig mapper;

        public User1C(UnitOfWork database, ILoggerFactory loggerFactory, TypeAdapterConfig mapper)
        {
            this.loggerFactory = loggerFactory;
            this.currentLogger = loggerFactory.CreateLogger(this.ToString());
            this.database = database;
            this.mapper = mapper;
        }



        public async Task<string> Create(Contact contact)
        {
            var dto1CContact = new SendPersonTo1CDTO();
            dto1CContact = contact.Adapt<SendPersonTo1CDTO>(mapper);

            if (dto1CContact.isValid == false) throw new ArgumentException(String.Join(" | ", dto1CContact.GetValidateErrors()));

            var guid = await database.Persons.Add2(dto1CContact.Adapt<AddPersonDTO>(mapper));

            return String.IsNullOrEmpty(guid) ? null : guid;
        }


        public async Task<string> SendLead(SendLeadto1CDTo lead)
        {
            var dto1CLead = new SendLeadto1CDTo();
            dto1CLead = lead.Adapt<SendLeadto1CDTo>(mapper);

            if (dto1CLead.isValid == false) throw new ArgumentException(String.Join(" | ", dto1CLead.GetValidateErrors()));

            var result = await database.Persons.InviteTo1C(dto1CLead.Adapt<AddLeadDTO>(mapper));

            if (result != "Студент зачислен") throw new ArgumentException();

            return String.IsNullOrEmpty(result) ? null : result;
        }



        public async Task<string> Find(Contact contact)
        {
            string guid = String.Empty;
            guid = await FindByPhones(contact.Phones().Select(x => x.Value));
            if(String.IsNullOrEmpty(guid)) guid = await FindByEmails(contact.Email().Select(x => x.Value));

            return guid;
        }

        private async Task<string> FindByPhones(IEnumerable<string> phones)
        {
            string guid = String.Empty;

            foreach (var phone in phones)
            {
                if (String.IsNullOrEmpty(guid))
                {
                    var chkPhone = phone.LeaveJustDigits();
                    guid = await GetGuid(chkPhone.Length >= 10 ? chkPhone.Substring(phone.Length - 10) : chkPhone, "");
                    if (!String.IsNullOrEmpty(guid)) return guid;
                }
            }
            return null;
        }

        private async Task<string> FindByEmails(IEnumerable<string> emails)
        {
            string guid = String.Empty;

            foreach (var email in emails)
            {
                if (String.IsNullOrEmpty(guid))
                {
                    guid = await GetGuid("", email.ClearEmail());
                    if (!String.IsNullOrEmpty(guid)) return guid;
                }
            }

            return null;
        }

        private async Task<string> GetGuid(string phone, string email)
        {
            flGUIDs query = null;

            try
            {
                query = await database.Persons.GetGuidByPhoneOrEmail(phone, email);
            }
            catch (Exception ex)
            {
                currentLogger.LogError(ex,"Ошибка, {@Message}, По адресу - {@Location}");
            }

            return query?.GUID;
        }
    }
}
