using Common.Extensions.ContactDomain;
using Domain.Models.Crm;
using Domain.Models.Crm.Parent;
using Library1C;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public class FindGuid : IDoAction<Contact>
    {
        readonly UnitOfWork database;
        readonly IDataManager crm;
        readonly TypeAdapterConfig mapper;

        public FindGuid(IDataManager crm, UnitOfWork database, TypeAdapterConfig mapper, ILoggerFactory loggerFactory)
        {
            this.database = database;
            this.crm = crm;
            this.mapper = mapper;
        }

        public async Task<Contact> DoActionAsync(Contact item)
        {
            var guid = await item.FindIn1C(database);
            if (!String.IsNullOrEmpty(guid))
            {
                item.Guid(guid);
                item.SetGuid(guid);
                await item.UpdateInCRM(crm, mapper);
            }

            return item;
        }
    }
}
