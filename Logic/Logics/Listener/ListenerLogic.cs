using Common.Extensions.ContactDomain;
using Common.Mapping;
using Library1C;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebApiLogic.Logics.Listener.DTO;
using WebApiLogic.Logics.Listener.EventHandlers;

namespace WebApiLogic.Logics.Listener
{
    public class ListenerLogic
    {
        CrmEventHandler events = new CrmEventHandler();

        TypeAdapterConfig mapper;
        IDataManager crm;
        UnitOfWork database;
        ILogger logger;

        IDoHandlerAction updatePhone;
        IDoHandlerAction linkTo1C;
        IDoHandlerAction send;

        public ListenerLogic(TypeAdapterConfig mapper, IDataManager crm, ILoggerFactory loggerFactory, UnitOfWork database)
        {
            this.mapper = mapper;
            new RegisterCommonMaps(mapper);
            this.crm = crm;
            this.database = database;
            this.logger = loggerFactory.CreateLogger(this.ToString());

            updatePhone = new UpdatePhone<WebApiLogic.Logics.Listener.Models.Contact>(crm, mapper, events, loggerFactory)
            {
                TypePredictions = x => x.Entity == "contacts",
                EntityPredictions = x => x.Phones().Count() > 0
            };
            events.Update += updatePhone.DoActionAsync;

            //send = new SendLeadTo1C(crm, mapper, loggerFactory, database)
            //{
            //    TypePredictions = x => x.Entity == "leads",
            //    EntityPredictions = x => x.Status == 555
            //};

            linkTo1C = new LinkTo1C<WebApiLogic.Logics.Listener.Models.Contact>(crm, database, mapper, loggerFactory)
            {
                TypePredictions = x => x.Entity == "contacts",
                EntityPredictions = x => x.Guid() == null & (x.Phones().Count > 0 || x.Email().Count >0)
            };
            events.Update += linkTo1C.DoActionAsync;
            events.Add += linkTo1C.DoActionAsync;
        }

        public void EventsHandle(IEnumerable<EventDTO> crmEvents)
        {
            foreach (var evnt in crmEvents)
            {
                switch (evnt.Event)
                {
                    case "add": events.OnAdd(evnt);      // !!!!!
                        break;
                    case "update": events.OnUpdate(evnt);
                        break;
                    case "ChangeResponsibleUser": events.OnResponsible(evnt);
                        break;
                    case "delete": events.OnDelete(evnt);
                        break;
                    case "note": events.OnNote(evnt);
                        break;
                    case "status": events.OnStatus(evnt);
                        break;
                    default: logger.LogInformation("Получено неизвестное\\новое событие - {Type}", evnt.Event);
                        break;
                }
            }
        }
    }
}
