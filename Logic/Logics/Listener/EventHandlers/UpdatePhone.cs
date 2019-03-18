using Common.Extensions;
using Common.Extensions.ContactDomain;
using Domain.Models.Crm.Parent;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiLogic.Logics.Listener.DTO;
using WebApiLogic.Logics.Listener.Models;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public class UpdatePhone<T> : HandlerBase<T> where T : Domain.Models.Crm.Contact
    {
        public UpdatePhone(IDataManager crm, TypeAdapterConfig mapper, ILoggerFactory loggerFactory)
            :base (crm, mapper, loggerFactory)
        {}

        public override async void DoActionAsync(object sender, EventDTO events)
        {
            if ( IsPassed(events) != true ) return;

            var toUpdate = (events.Entities ?? new List<EntityCore>()).Cast<T>().Where(EntityPredictions);

            foreach (var item in toUpdate)
            {
                foreach (var phone in item.Phones() ?? new Dictionary<LibraryAmoCRM.Configuration.PhoneTypeEnum, string>())
                {
                    if (phone.Value != phone.Value.LeaveJustDigits())
                    {
                        item.Phones(phone.Key, phone.Value.LeaveJustDigits());
                    }

                    try
                    {
                        if (item.ChangeValueDelegate != null)
                        {
                            var dto = item.GetChanges().Adapt<ContactDTO>(mapper);
                            await crm.Contacts.Update(dto);
                            logger.LogInformation("Обновлено значения телефона пользователя. [ ID - {Id} | Name - {Name} | Phone - {Phone}]", item.Id, item.Name, phone.Value.LeaveJustDigits());
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Ошибка обновления телефона пользователя. [{@Id}]", item.Id);
                        break;
                    }
                }
            }
        }


    }
}
