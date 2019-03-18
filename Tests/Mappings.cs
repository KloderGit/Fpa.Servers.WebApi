using Common.DTO.Service1C;
using Common.Extensions;
using Library1C;
using Library1C.DTO;
using Mapster;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Controllers.SignUp.Models;
using WebApi.Infrastructure.LocalMaps;

namespace WebApiTests
{
    [TestClass]
    public class Mappings
    {
        [TestMethod]
        public void SendLeadto1CDTo_AddLeadDTO()
        {
            var commonDTO = new SendLeadto1CDTo
            {
                ProgramGuid = "7da38f8f-9f71-11e6-80e7-0cc47a4b75cc", // 3a660dca-9f7b-11e6-80e7-0cc47a4b75cc
                UserGuid = "cef55369-cd46-11e8-8103-0cc47a4b75cc", //5bf0c9ee-8973-11e6-8102-10c37b94684b
                ContractTitle = "ТстДГВор",
                ContractGroup = "Тест",
                ContractEducationStart = "2018-10-10".ToDateTime('-'),
                ContractEducationEnd = "2018-10-10".ToDateTime('-').AddDays(10),
                ContractExpire = "2018-10-10".ToDateTime('-').AddDays(180),
                ContractPrice = 8000,
                DecreeTitle = "ТстПрикз"
            };

            var serviceDTO = commonDTO.Adapt<AddLeadDTO>();

            Assert.AreEqual(serviceDTO.ProgramGuid, "7da38f8f-9f71-11e6-80e7-0cc47a4b75cc");
            Assert.AreEqual(serviceDTO.ContractSubGroup,"");
            Assert.AreEqual(serviceDTO.ContractEducationEnd, new DateTime(2018,10,20));
        }

        [TestMethod]
        public void SendLead2()
        {
            var commonDTO = new SendLeadto1CDTo
            {
                ContractTitle = "ТстДГВор",
                //ContractGroup = "Тест",
                //ContractEducationStart = "2019-03-10".ToDateTime('-'),
                //ContractEducationEnd = "2019-03-13".ToDateTime('-').AddDays(10),
                //ContractExpire = "2019-03-20".ToDateTime('-').AddDays(180),
                ContractPrice = 8000,
                //DecreeTitle = "ТстПрикз",
                ProgramGuid = "3a660dca-9f7b-11e6-80e7-0cc47a4b75cc",
                UserGuid = "5bf0c9ee-8973-11e6-8102-10c37b94684b"
            };

            var serviceDTO = commonDTO.Adapt<AddLeadDTO>();

            var sdf = new UnitOfWork("Kloder", "Kaligula2");

            //  var sss = sdf.Persons.InviteTo1C(serviceDTO).Result;

        }


        [TestMethod]
        public void MapModel()
        {
            IEnumerable<SiteFormField> fullData = new List<SiteFormField> {
                new SiteFormField{ Name = "pay", Value = "some value" },
                new SiteFormField{ Name = "ID", Value = "6868" },
                new SiteFormField{ Name = "DATA[EDU_NAME]", Value = "Анатомия мышечной системы: практическое занятие на макете" },
                new SiteFormField{ Name = "DATA[EDU_TYPE]", Value =  "Дистанционное" },
                new SiteFormField{ Name = "DATA[PRICE]", Value = "15000" },
                new SiteFormField{ Name = "TYPE", Value = "Семинары" },
                new SiteFormField{ Name = "GUID_EVENT", Value = "654654-987" },
                new SiteFormField{ Name = "DATA[SUBJECT]", Value = "F" },
                new SiteFormField{ Name = "DATA[NAME]", Value =  "Илья" },
                new SiteFormField{ Name = "DATA[PHONE][]" , Value = "89031453412" },
                new SiteFormField{ Name = "DATA[EMAIL][]", Value = "kloder3@gmail.com" },
                new SiteFormField{ Name = "DATA[CITY]", Value =  "Москва" },
                new SiteFormField{ Name = "DATA[DATE]", Value =  "18.02.2019" },
                new SiteFormField{ Name = "DATA[AGREE]", Value =  "1" },
                new SiteFormField{ Name = "sessid", Value =  "18bdeb34311d9a1b884127f52f4e9b77" }
            };

            var map = new TypeAdapterConfig();
            Map_FormToModel m = new Map_FormToModel( map );

            var model = fullData.Adapt<SiteFormModel>( map );

            Assert.AreEqual( model.Contact.City, "Москва" );
            Assert.AreEqual( model.Contact.Email, "kloder3@gmail.com" );
            Assert.AreEqual( model.Contact.Name, "Илья" );
            Assert.AreEqual( model.Contact.Phone, "89031453412" );

            Assert.AreEqual( model.Lead.Agree, true );
            Assert.AreEqual( model.Lead.Date, new DateTime( 2019, 2, 18 ) );
            Assert.AreEqual( model.Lead.EducationForm, "Дистанционное" );
            Assert.AreEqual( model.Lead.EducationType, "Семинары" );
            Assert.AreEqual( model.Lead.Guid, "654654-987" );
            Assert.AreEqual( model.Lead.isPerson, true );
            Assert.AreEqual( model.Lead.Name, "Анатомия мышечной системы: практическое занятие на макете" );
            Assert.AreEqual( model.Lead.Pay, "some value" );
            Assert.AreEqual( model.Lead.Price, 15000 );


            IEnumerable<SiteFormField> minData = new List<SiteFormField>();

            var model2 = minData.Adapt<SiteFormModel>( map );

            Assert.AreEqual( model2.Lead.Agree, false );
            Assert.AreEqual( model2.Lead.Price, 0 );
            Assert.AreEqual( model2.Lead.Name, null );
            Assert.AreEqual( model2.Lead.Date, default( DateTime ) );
        }

    }
}