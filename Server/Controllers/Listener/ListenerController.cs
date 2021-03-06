﻿using Common.Mapping;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Common.Models;
using WebApi.Controllers.Listener.LocalMaps;
using WebApi.Controllers.Listener.Models;
using WebApi.Infrastructure.Filters;
using WebApiLogic.Logics.Listener;
using WebApiLogic.Logics.Listener.DTO;
using System.Web;

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
            logger.LogDebug("FirstDVS | Событие AmoCRM [ {Entity} | {Event} ] -- Model {@model}", value.First().Entity, value.First().Event, value.First());

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

        [HttpPost]
        [Route("raw")]
        public HttpResponseMessage Post()
        {
            var incomingQuery = new StreamReader(Request.Body).ReadToEndAsync().Result;

            logger.LogInformation("FirstDVS | Source | Данные от AMO, {Data}", HttpUtility.UrlDecode(incomingQuery.ToString()));

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("getguid")]
        public async Task<IActionResult> GetOrCreateUser1CGuid(int id)
        {
            string result = default;

            try
            {
                result = await logic.GetOrCreate1CUserFromCrm(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return new ObjectResult(result)
            {
                StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status200OK
            };
        }
    }
}