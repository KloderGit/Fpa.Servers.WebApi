using Common.Interfaces;
using Common.Logging;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using WebApi.Common.Models;
using WebApi.Utils;
using WebApiBusinessLogic;
using WebApiBusinessLogic.Infrastructure;
using WebApiBusinessLogic.Models.Crm;

namespace WebApiFPA.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Listener2")]
    public class ListenerController : Controller
    {
        BusinessLogic logic;
        ILoggerFactory loggerFactory;
        Microsoft.Extensions.Logging.ILogger logger;
        TypeAdapterConfig mapper;

        public ListenerController(ILoggerFactory loggerFactory, BusinessLogic logic, TypeAdapterConfig mapper)
        {
            new RegisterMapsterConfig();

            this.loggerFactory = loggerFactory;
            this.logger = loggerFactory.CreateLogger(this.ToString());
            this.logic = logic;
            this.mapper = mapper;
        }

        // POST api/Listener
        [HttpPost]
        //public HttpResponseMessage Post()
        public HttpResponseMessage Post(ListenedEvent value)
        //public HttpResponseMessage Post(
        //    Dictionary<string, 
        //        Dictionary<string, 
        //            Dictionary<int, 
        //                Dictionary<string,string>>>>
        //    value)
        {

            logger.LogInformation("Входящее событие, {@Element}", value);

            logic.GetEvent(value.Adapt<CrmEvent>());

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // POST api/Listener/raw
        [HttpPost]
        [Route("raw")]
        public HttpResponseMessage Post()
        {
            var ttt = new StreamReader(Request.Body).ReadToEndAsync().Result;

            logger.LogInformation("Данные от AMO, {Data}", ttt.ToString());

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route( "raw2" )]
        public HttpResponseMessage Post(dynamic model)
        {

            var ttt = new StreamReader( Request.Body ).ReadToEndAsync().Result;

            logger.LogInformation( "Данные от AMO, {Data}", ttt.ToString() );

            return new HttpResponseMessage( HttpStatusCode.OK );
        }

    }

}
