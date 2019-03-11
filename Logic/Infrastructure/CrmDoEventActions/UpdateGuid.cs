using Common.BusinessLogicHelpers.Crm.Actions;
using Common.BusinessLogicHelpers.IcActions;
using Common.Extensions.ContactDomain;
using Domain.Models.Crm;
using Library1C;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using WebApiLogic.Models.Crm;

namespace WebApiLogic.Infrastructure.CrmDoEventActions
{
    public class UpdateGuid
    {
        IDataManager crm;
        UnitOfWork database;

        TypeAdapterConfig mapper;

        ILoggerFactory loggerFactory;
        ILogger currentLogger;

        public UpdateGuid(IDataManager amocrm, UnitOfWork database, CrmEventTypes @Events, TypeAdapterConfig mapper, ILoggerFactory loggerFactory)
        {
            this.mapper = mapper;

            this.loggerFactory = loggerFactory;
            this.currentLogger = loggerFactory.CreateLogger(this.ToString());

            this.database = database;
            this.crm = amocrm;

            Events.Update += DoAction;
            Events.Add += DoAction;
        }

        public async void DoAction(object sender, CrmEvent e)
        {
            if (e.Entity != "contacts" || String.IsNullOrEmpty(e.EntityId) || e.ContactType != "contact") return;

            Contact contact = null;

            try
            {
                var idConvert = int.TryParse(e.EntityId, out int id);

                if (idConvert)
                {
                    var action = new FindContactActions(crm, currentLogger);

                    var result = await action.LookForContact(id);
                    contact = result?.Adapt<Contact>(mapper);
                }                
            }
            catch (Exception ex)
            {
                currentLogger.LogError(ex, "Ошибка при поиске контакта в Crm");
                return;
            }

            if (contact == null)
            {
                currentLogger.LogWarning("Контакт [ Id -" + e.EntityId + " ] не найден в CRM");
                return;
            }
                

            var hasGuid = contact.Guid();
            if (!String.IsNullOrEmpty(hasGuid)) return;

            try
            {
                var guid = await new FindGuidAction(database).Find(contact);

                if (!String.IsNullOrEmpty(guid))
                {
                    contact.Guid(guid);

                    await crm.Contacts.Update(
                        contact.GetChanges().Adapt<ContactDTO>(mapper)
                    );

                    currentLogger.LogInformation("Обновление Guid - {Guid}, для пользователя Id - {User}", guid, contact.Id);
                }
            }
            catch (NullReferenceException ex)
            {
                currentLogger.LogDebug(ex, "Ошибка, нулевое значение {@Contacts}", contact);
                return;
            }
            catch (Exception ex)
            {
                currentLogger.LogError(ex, "Ошибка обновления пользователя. [{@Id}]", contact.Id);
            }

        }
    }
}
