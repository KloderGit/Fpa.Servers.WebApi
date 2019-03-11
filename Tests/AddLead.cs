using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WebApi.Controllers.SignUp;
using WebApi.Controllers.SignUp.Models;
using WebApi.Infrastructure.LocalMaps;

namespace WebApiTests
{
    [TestClass]
    public class AddLead_Metod
    {

        [TestMethod]
        public void ValidateModel()
        {
            IEnumerable<SiteFormField> data = new List<SiteFormField> {
                new SiteFormField{ Name = "DATA[EDU_NAME]", Value = "Анатомия мышечной системы: практическое занятие на макете" },
                new SiteFormField{ Name = "DATA[NAME]", Value =  "Илья" },
                new SiteFormField{ Name = "DATA[PHONE][]" , Value = "89031453412" },
                new SiteFormField{ Name = "DATA[EMAIL][]", Value = "kloder3@gmail.com" },
                new SiteFormField{ Name = "DATA[PRICE]", Value = "" }
            };

            var map = new TypeAdapterConfig();
            Map_FormToModel m = new Map_FormToModel( map );

            var controller = new SignUpController(map, null, null );

            var result = controller.GivenFromSiteForm( data );

            Assert.IsInstanceOfType( result.Result, typeof( OkResult ) );

            IEnumerable<SiteFormField> data2 = new List<SiteFormField> {
                new SiteFormField{ Name = "DATA[NAME]", Value =  "Илья" },
                new SiteFormField{ Name = "DATA[EMAIL][]", Value = "kloder3@gmail.com" }
            };

            var controller2 = new SignUpController( map, null, null );

            dynamic result2 = controller.GivenFromSiteForm( data2 );

            Assert.IsInstanceOfType( result2.Result, typeof( BadRequestObjectResult ) );

            Assert.AreEqual( result2.Result.Value.Count, 1 );

            Assert.AreEqual( result2.Result.Value[ "Данные формы" ][ 0 ], "Не указан телефон пользователя" );
        }
    }
}
