using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiLogic.Logics.Listener.DTO;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public abstract class HandlerBase<T> : IDoHandlerAction
    {
        public Predicate<EventDTO> TypePredictions;
        public Func<T, bool> EntityPredictions;

        protected IDataManager crm;
        protected TypeAdapterConfig mapper;
        protected ILogger logger;

        public HandlerBase(IDataManager crm, TypeAdapterConfig mapper, ILoggerFactory loggerFactory)
        {
            this.crm = crm;
            this.mapper = mapper;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }

        public abstract void DoActionAsync(object sender, EventDTO events);

        protected bool IsPassed(EventDTO events)
        {
            return TypePredictions == null ? true : TypePredictions(events);
        }
    }
}
