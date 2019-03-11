using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using WebApi.Common.Models;
using WebApi.Controllers.CallBack.Models;
using WebApi.Infrastructure.Filters;
using WebApiLogic.Logics.CallBack;
using WebApiLogic.Logics.CallBack.Models;

namespace WebApi.Controllers.CallBack
{
    //[TypeFilter(typeof(RequestScopeFilter))]
    [Produces("application/json")]
    [Route("CallBack")]
    public class CallBackController : Controller
    {
        TypeAdapterConfig mapper;
        ILogger logger;

        CallBackLogic logic;

        public CallBackController(TypeAdapterConfig mapper, IDataManager crm, ILoggerFactory loggerFactory)
        {
            this.mapper = mapper;
            this.logger = loggerFactory.CreateLogger(this.ToString());

            this.logic = new CallBackLogic(mapper, crm, loggerFactory);
        }


        [HttpPost]
        [Route("request")]
        public async Task<IActionResult> GetModel([FromBody] CallBackViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            logger.LogInformation("Получена форма на запрос Перезвонить {@Model}", model);

            var res = await logic.CreateRecallTask(model.Adapt<CallBackDTO>());

            if (!res) return StatusCode(500);

            return Ok();
        }
    }
}