using Common.Extensions;
using Common.Extensions.ContactDomain;
using Domain.Models.Crm;
using LibraryAmoCRM.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ListenerModels = WebApiBusinessLogic.Logics.Listener.Models;
using DomainModels = Domain.Models.Crm;
using LibraryAmoCRM.Interfaces;
using Library1C;
using Mapster;
using Common.Mapping;
using System.Linq;

namespace WebApiTests
{
    [TestClass]
    public class UpdatePhoneTest
    {
        Services Services = new Services();

        TypeAdapterConfig mapper;
        IDataManager Crm;
        UnitOfWork IC;

        public UpdatePhoneTest()
        {
            mapper = Services.GetMapper();
            Crm = Services.GetCRM();
            IC = Services.Get1C();

            new Domain_AmoCRM(mapper);
            new Domain_1C(mapper);
        }

        [TestMethod]
        public void CreateUserIn1C()
        {
            var action = new WebApiBusinessLogic.Logics.Listener.EventHandlers.UpdatePhone<Contact>(Crm,mapper,null,null)
            {
                TypePredictions = x => x.Entity == "contacts",
                EntityPredictions = x => x.Phones().Count() > 0
            };




            var contact = GetContact();


        }

        ListenerModels.Contact GetContact()
        {
            ListenerModels.Contact contact = new ListenerModels.Contact();

            contact.AccountId = 17769199;
            contact.ClosestTaskAt = DateTime.MinValue;
            contact.CreatedAt = DateTime.Now;
            contact.CreatedBy = 0;
            contact.GroupId = 0;
            contact.Id = 22309159;
            contact.Name = "Тестовое Илья Юрьевич";
            contact.ResponsibleUserId = 2079718;
            contact.UpdatedAt = DateTime.Now;
            contact.UpdatedBy = 2079718;

            contact.Tags = new List<DomainModels.Fields.Tag> {
                new DomainModels.Fields.Tag { Id = 72289, Name = "Заявка с сайта" },
                new DomainModels.Fields.Tag { Id = 176263, Name = "callback"}
            };

            contact.Leads = new List<DomainModels.Lead> {
                new ListenerModels.Lead { Id = 9982719 },
                new ListenerModels.Lead { Id = 10362151 },
                new ListenerModels.Lead { Id = 10374575 }
            };

            contact.Company = new Company { Id = 22797025, Name = "АССОЦИАЦИЯ ПРОФЕССИОНАЛОВ ФИТНЕСА" };

            contact.City("Москва");
            contact.Agreement(false);
            contact.Birthday("1978/02/02".ToDateTime('/'));
            contact.Education("Высшее");
            contact.Experience("5 лет");
            contact.GroupPart("2");

            contact.Location("Avtozavod");
            contact.MailChimp(true);

            contact.Phones(PhoneTypeEnum.MOB, "89998887777");
            contact.Position("Developer");

            contact.Fields = contact.GetChanges().Fields;

            return contact;
        }
    }
}
