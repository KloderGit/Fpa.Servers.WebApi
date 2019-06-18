using Common.Extensions.ContactDomain;
using Common.Extensions.LeadDomain;
using Domain.Models.Crm.Parent;
using Library1C;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WebApiLogic.Logics.Listener.DTO;
using WebApiLogic.Logics.Listener.Models;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public class SendLeadTo1C : HandlerBase<Lead>
    {
        UnitOfWork database;
        ILoggerFactory loggerFactory;

        public SendLeadTo1C(IDataManager crm, TypeAdapterConfig mapper, ILoggerFactory loggerFactory, UnitOfWork service1C)
            :base (crm, mapper, loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.logger = loggerFactory.CreateLogger(this.ToString());
            this.database = service1C;
        }

        public override async void DoActionAsync(object sender, EventDTO events)
        {
            if ( IsPassed(events) != true ) return;

            var toUpdate = events.Entities ?? new List<EntityCore>();

            foreach (var itemCore in toUpdate)
            {
                var lead = itemCore as Lead;
                logger.LogDebug("Отправка сделки в 1С. Source - {@Model}", lead);
                
                lead.GetSelf(crm, mapper);

                var contact = lead.MainContact;
                contact.GetSelf(crm, mapper);

                if (String.IsNullOrEmpty(contact.Guid()))
                {
                    var guid = contact.FindIn1C(database).Result;

                    if (String.IsNullOrEmpty(guid))
                    {
                        guid = contact.CreateIn1C(database, mapper).Result;
                        contact.Guid(guid); contact.SetGuid(guid);
                        await contact.UpdateInCRM(crm, mapper);
                    }
                    else
                    {
                        contact.Guid(guid); contact.SetGuid(guid);
                        await contact.UpdateInCRM(crm, mapper);
                    }
                }

                try
                {
                    var result = await lead.SendTo1C(database, mapper);

                    if (!String.IsNullOrEmpty(result))
                    {
                        lead.IsInService1C(true);
                        await crm.Leads.Update(lead.GetChanges().Adapt<LeadDTO>(mapper));

                        //lead.AddNote(NoteType.SYSTEM, "Сделка успешно отправлена в 1С.", crm);     
                        lead.AddNote(NoteType.COMMON, "Сделка успешно отправлена в 1С.", crm);

                        logger.LogInformation("Сделка Id - {LeadID} успешно отправлена в 1С", lead.Id);
                    }
                }
                catch (ArgumentException ex)
                {
                    lead.AddNote(NoteType.COMMON, "Внимание, сделка не отправлена в 1С. По причине - " + "\r\n" + ex.Message, crm);
                }
                catch (InvalidOperationException ex)
                {
                    lead.AddNote(NoteType.COMMON, "Внимание, сделка не отправлена в 1С. По причине - " + "\r\n" + ex.Message, crm);
                }
                catch (NullReferenceException ex)
                {
                    logger.LogWarning(ex, "Получено нулевое значение");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Ошибка отправки сделки в 1С");
                }
            }
        }
    }
}
