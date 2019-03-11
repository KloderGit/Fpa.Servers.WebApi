using Common.Extensions;
using Domain.Models.Crm;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLogic.Infrastructure.Actions
{
    public class AmoCRMCommonActions
    {
        ILogger logger;
        IDataManager amoManager;
        TypeAdapterConfig mapper;

        public AmoCRMCommonActions(IDataManager amoManager, TypeAdapterConfig mapper, ILogger logger)
        {
            this.logger = logger;
            this.amoManager = amoManager;
            this.mapper = mapper;
        }

        public async Task<Contact> LookForContact(IEnumerable<string> phones, IEnumerable<string> emails, string guid)
        {
            Contact query = null; ;

            if (!String.IsNullOrEmpty(guid))
            {
                query = await FindContactByGUID(guid);
                if (query != null) return query;
            }

            if (phones != null && phones.Count() > 0)
            {
                query = await FindContact(ClearPhones(phones));
                if (query != null) return query;
            }

            if (emails != null && emails.Count() > 0)
            {
                query = await FindContact(ClearEmails(emails));
            }

            return query;

            IEnumerable<string> ClearPhones(IEnumerable<string> values)
            {
                var array = values.ToList();
                for (var i = 0; i < array.Count(); i++) { array[i] = array[i].LeaveJustDigits(); }
                return array;
            }
            IEnumerable<string> ClearEmails(IEnumerable<string> values)
            {
                var array = values.ToList();
                for (var i = 0; i < array.Count(); i++) { array[i] = array[i].ClearEmail(); }
                return array;
            }
        }

        public async Task<Contact> LookForContact(string phone = "", string email = "", string guid = "")
        {
            Contact query = null; ;

            if (!String.IsNullOrEmpty(guid))
            {
                query = await FindContactByGUID(guid);
                if (query != null) return query;
            }

            if (!String.IsNullOrEmpty(phone))
            {
                query = await FindContact(phone.PhoneWithoutCode());
                if (query != null) return query;
            }

            if (!String.IsNullOrEmpty(email))
            {
                query = await FindContact(email.ClearEmail());
            }

            return query;
        }

        public async Task<Contact> FindContact(IEnumerable<string> queryParams)
        {
            if (queryParams == null || queryParams.Count() == 0) return null;

            var query = amoManager.Contacts.Get();

            foreach (var param in queryParams)
            {
                query.Filter(i=>i.Query = param);
            }
            
            var result = await query.Execute();

            return result?.FirstOrDefault().Adapt<Contact>(mapper);
        }

        public async Task<Contact> FindContact(string queryParam)
        {
            if ( String.IsNullOrEmpty(queryParam) ) return null;

            var query = amoManager.Contacts.Get().Filter(p=>p.Query = queryParam);
            var result = await query.Execute();

            return result?.FirstOrDefault().Adapt<Contact>(mapper);
        }

        public async Task<Contact> FindContactByGUID(string guid)
        {
            if (String.IsNullOrEmpty( guid )) return null;

            var query = amoManager.Contacts.Get().Filter( p => p.Query = guid );
            var result = await query.Execute();

            return result?.FirstOrDefault().Adapt<Contact>( mapper );
        }
    }
}
