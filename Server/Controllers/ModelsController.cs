using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 600)]
        [Route("odataprograms")]
        public JToken GetODataPrograms()
        {
            JToken value;

            using (var client = new HttpClient())
            {

                var urll = @"http://217.16.28.213/1CDB_FPA_DPO/odata/standard.odata/Catalog_%D0%A6%D0%B8%D0%BA%D0%BB%D1%8B%D0%94%D0%9F%D0%9E?$format=json&$select=Ref_Key, ПолноеНаименование, Статус, ГруппаПрограммыОбучения_Key, ФормаОбучения_Key&$filter=Статус eq 'Активный'";

                var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"Kloder:Kaligula2")));
                client.DefaultRequestHeaders.Authorization = authValue;

                var response = client.GetAsync(urll).Result;
                var result = response.Content.ReadAsAsync<JObject>().Result;

                value = result.GetValue("value");
            }

            return value;
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
