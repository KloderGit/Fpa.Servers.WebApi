using Common.Mapping;
using Library1C;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Common.Models;
using WebApi.Controllers.Listener.LocalMaps;
using WebApi.Controllers.Listener.Models;
using WebApi.Infrastructure.Filters;
using WebApiLogic.Logics.Listener;
using WebApiLogic.Logics.Listener.DTO;

namespace WebApi.Controllers.Listener
{
    [Produces("application/json")]
    [Route("api/Listener")]
    [TypeFilter(typeof(RequestScopeFilter))]
    public class ListenerController : Controller
    {
        TypeAdapterConfig mapper;
        Microsoft.Extensions.Logging.ILogger logger;

        ListenerLogic logic;

        public ListenerController(TypeAdapterConfig mapper, ILoggerFactory loggerFactory, IDataManager crm, UnitOfWork service1C, RequestScope requestScope)
        {
            this.mapper = mapper;
            new RegisterCommonMaps(mapper);
            new IncomingEventToDTO(mapper);

            this.logger = loggerFactory.CreateLogger(this.ToString());

            logic = new ListenerLogic(mapper, crm, loggerFactory, service1C);
        }

        [HttpPost]
        public async Task<IActionResult> Post([ModelBinder(typeof(EventsModelBinder))] IEnumerable<EventViewModel> value)
        {
            logger.LogDebug("Azure | Событие AmoCRM [ {Entity} | {Event} ] -- Model {@model}", value.First().Entity, value.First().Event, value.First());

            try
            {
                var model = value.Adapt<IEnumerable<EventDTO>>(mapper);

                await logic.EventsHandle(model);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Ошибка обработки модели {@model}", value);
                return StatusCode(500);
            }

            return Ok();
        }
    }
}