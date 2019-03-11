using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiLogic;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Models")]
    public class ModelsController : Controller
    {
        BusinessLogic logic;
        Microsoft.Extensions.Logging.ILogger logger;
        ILoggerFactory loggerFactory;
        IMemoryCache cache;

        public ModelsController(ILoggerFactory loggerFactory, BusinessLogic logic, IMemoryCache memoryCache)
        {
            this.loggerFactory = loggerFactory;
            this.logger = loggerFactory.CreateLogger(this.ToString());
            this.logic = logic;
            this.cache = memoryCache;
        }

        [ResponseCache( Location = ResponseCacheLocation.Client, Duration = 30 )]
        [Route("programs")]
        public JArray GetPrograms()
        {
            var result = cache.GetOrCreate( "Programs", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours( 20 );

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var vm = new JArray();

                foreach (var cycle in logic.GetProgramsListForAmo())
                {
                    var cycleJson = JObject.FromObject( cycle );
                    vm.Add( cycleJson );
                }

                return vm;
            } );

            return result;
        }


        [Route("status/{status?}")]
        public async Task<IEnumerable<string>> Get(int status)
        {
            // 17769208

            var leads = await logic.GetLeadsByStatus(status);

            return leads.ToList().Where(i => i.IsDeleted != true).Select(i => i.Name);
        }

        public string Get()
        {
            return "value";
        }

    }
}
