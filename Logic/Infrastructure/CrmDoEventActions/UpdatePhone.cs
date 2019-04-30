using Common.Extensions;
using Common.Extensions.ContactDomain;
using Domain.Models.Crm;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiLogic.Models.Crm;

namespace WebApiLogic.Infrastructure.CrmDoEventActions
{
    public class UpdatePhone
    {
        TypeAdapterConfig mapper;

        ILoggerFactory loggerFactory;
        ILogger currentLogger;

        IDataManager crm;

        public UpdatePhone(IDataManager crm, CrmEventTypes @Events, TypeAdapterConfig mapper, ILoggerFactory loggerFactory)
        {
            this.mapper = mapper;
            this.loggerFactory = loggerFactory;
            this.currentLogger = loggerFactory.CreateLogger(this.ToString()); this.crm = crm;

            Events.Update += DoAction;
            //Events.Add += DoAction;
        }

        public async void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "contacts" || String.IsNullOrEmpty( e.EntityId ) || e.ContactType != "contact") return;

            IEnumerable<ContactDTO> amoUser = null;
            Contact contact = null;

            try
            {
                var id = int.Parse(e.EntityId);

                amoUser = await crm.Contacts.Get().Filter(f => f.Id = id ).Execute();
                if (amoUser == null) throw new NullReferenceException("Контакт [ Id -" + e.EntityId + " ] не найден в CRM");

                contact = amoUser.Adapt<IEnumerable<Contact>>(mapper).FirstOrDefault();
            }
            catch (NullReferenceException ex)
            {
                currentLogger.LogDebug(ex, "Ошибка, нулевое значение {@Contacts}", contact, amoUser);
                return;
            }
            catch (ArgumentNullException ex)
            {
                currentLogger.LogDebug(ex, "Ошибка (int) конвертации ID {@Contact}", contact);
                return;
            }
            catch (Exception ex)
            {
                currentLogger.LogDebug(ex, "Запрос пользователя amoCRM окончился неудачей. Событие - {@Event}, {@AmoUser}", e, amoUser, contact);
                return;
            }

            foreach (var phone in contact.Phones())
            {
                if (!( phone.Value == phone.Value.LeaveJustDigits() ))
                {
                    contact.Phones( phone.Key, phone.Value.LeaveJustDigits() );
                }
            }

            try
            {
                if (contact.ChangeValueDelegate != null)
                {
                    var dto = contact.GetChanges().Adapt<ContactDTO>( mapper );
                    await crm.Contacts.Update( dto );

                    currentLogger.LogInformation( "Обновление Phone для пользователя Id - {User}", contact.Id );
                }
            }
            catch (Exception ex)
            {
                currentLogger.LogDebug( ex, "Ошибка обновления пользователя. [{@Id}]", contact.Id );
            }

        }

    }
}
