using Common.BusinessLogicHelpers.IcActions;
using Common.Extensions.ContactDomain;
using Domain.Models.Crm.Parent;
using Library1C;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiLogic.Logics.Listener.DTO;
using WebApiLogic.Logics.Listener.Models;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public class LinkTo1C<T> : HandlerBase<T> where T : Domain.Models.Crm.Contact
    {
        readonly UnitOfWork database;

        public LinkTo1C(IDataManager crm, UnitOfWork database, TypeAdapterConfig mapper, ILoggerFactory loggerFactory)
            : base(crm, mapper, loggerFactory)
        {
            this.database = database;
        }

        public override async void DoActionAsync(object sender, EventDTO events)
        {
            if (IsPassed(events) != true) return;

            var toUpdate = (events.Entities ?? new List<EntityCore>()).Cast<T>().Where(EntityPredictions);

            foreach (var item in toUpdate)
            {
                try
                {
                    var guid = await item.FindIn1C(database);

                    item.Guid(guid);

                    await item.UpdateInCRM(crm, mapper);

                    //logger.LogInformation("Обновлено значения GUID пользователя. [ ID - {Id} | Name - {Name} | GUID - {Guid}]", item.Id, item.Name, guid);
                }
                catch (ArgumentNullException ex) { logger.LogWarning(ex, ex.Message); }
                catch (NullReferenceException ex) { logger.LogWarning(ex, ex.Message); }
                catch (Exception ex) { logger.LogWarning(ex, "Ошибка при обновлении GUID"); }
            }
        }
    }
}
