using Common.Interfaces;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.SignUp.Models;
using WebApi.Infrastructure.Binders;
using WebApi.Infrastructure.Filters;
using WebApi.Infrastructure.LocalMaps;
using WebApiLogic.Logics.SignUp;
using WebApiLogic.Logics.SignUp.Model;

namespace WebApi.Controllers.SignUp
{
    //[TypeFilter(typeof(RequestScopeFilter))]
    [Produces("application/json")]
    [Route("SignUp")]
    public class SignUpController : Controller
    {
        SignUpLogic logic;
        TypeAdapterConfig mapper;
        ILogger logger;

        public SignUpController(TypeAdapterConfig mapper, IDataManager crm, ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger(this.ToString());

            this.mapper = mapper;
                new Map_FormToModel( mapper );
            this.logic = new SignUpLogic(mapper, crm, loggerFactory);
        }

        //[HttpPost]
        //[Route("LeadFromSiteForm")]
        //public IActionResult SiteForm()
        //{
        //    var ttt = new StreamReader(Request.Body).ReadToEndAsync().Result;

        //    logger.LogInformation("Форма с сайта, {Data}", ttt.ToString());

        //    return Ok();
        //}


        [HttpPost]
        [Route( "LeadFromSiteForm" )]
        public async Task<IActionResult> GivenFromSiteForm([FromBody]IEnumerable<SiteFormField> fields)
        {
            SiteFormModel model = null;

            // Convert to Model
            try
            {
                model = fields.Adapt<SiteFormModel>( mapper );

                logger.LogInformation("Получена модель с форм сайта {@Model}", model);
            }
            catch (Exception ex)
            {
                logger.LogError( ex, "Ошибка маппинга модели данных формы с сайта {@Model}", new { Params = fields } );
            }
            
            // Check Model
            var validateResults = new List<ValidationResult>();
            var context = new ValidationContext( model );
            if (!Validator.TryValidateObject( model, context, validateResults, true ))
            {
                foreach (var error in validateResults)
                {
                    ModelState.AddModelError( "errors", error.ErrorMessage );
                }

                logger.LogError("Модель не прошла валидацию - {@Errors}", validateResults.Select(e=>e.ErrorMessage));

                return BadRequest( ModelState );
            }


            SignUpDTO dto = null;

            try
            {
                dto = model.Adapt<SignUpDTO>();
            }
            catch (Exception ex)
            {
                logger.LogError( ex, "Ошибка маппинга модели данных формы в модель DTO {@Model}", model );
            }

            try
            {
                var result = await logic.AddLead( dto );
                return Created( "", result );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError( "errors", ex.Message );
                logger.LogError(ex, "Ошибка в логике создания сделки", model);
                return BadRequest( ModelState );
            }
            
        }
    }
}
