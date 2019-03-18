using Common.Extensions.ContactDomain;
using Common.Mapping;
using Library1C;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiLogic.Logics.Listener.DTO;
using WebApiLogic.Logics.Listener.EventHandlers;

namespace WebApiLogic.Logics.Listener
{
    public class ListenerLogic
    {
        CrmEventHandler events = new CrmEventHandler();
        readonly TypeAdapterConfig mapper;
        readonly IDataManager crm;
        readonly UnitOfWork database;
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

            updatePhone = new UpdatePhone<WebApiLogic.Logics.Listener.Models.Contact>(crm, mapper, loggerFactory)
            {
                TypePredictions = x => x.Entity == "contacts",
                EntityPredictions = x => x.Phones().Count() > 0
            };
            events.Update += updatePhone.DoActionAsync;

            send = new SendLeadTo1C(crm, mapper, loggerFactory, database)
            {
                TypePredictions = x => x.Entity == "leads",
                //EntityPredictions = x => x.Status == 19368232 && x.Pipeline.Id == 1102975
            };
            events.Status += send.DoActionAsync;

            linkTo1C = new LinkTo1C<WebApiLogic.Logics.Listener.Models.Contact>(crm, database, mapper, loggerFactory)
            {
                TypePredictions = x => x.Entity == "contacts",
                EntityPredictions = x => x.Guid() == null & (x.Phones().Count > 0 || x.Email().Count >0)
            };
            events.Update += linkTo1C.DoActionAsync;
            events.Add += linkTo1C.DoActionAsync;
        }

        public async Task EventsHandle(IEnumerable<EventDTO> crmEvents)
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
