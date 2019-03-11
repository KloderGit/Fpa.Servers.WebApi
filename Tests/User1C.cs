using Common.Extensions;
using Common.Extensions.ContactDomain;
using Common.Mapping;
using Domain.Models.Crm;
using Library1C;
using LibraryAmoCRM.Configuration;
using Mapster;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiBusinessLogic.Infrastructure.Actions;

namespace WebApiTests
{
    [TestClass]
    public class User1CTest
    {
        [TestMethod]
        public void CreateUserIn1C()
        {
            var database = new UnitOfWork("Kloder", "Kaligula2");
            //var logger = new LoggerService();
            TypeAdapterConfig mapper = new TypeAdapterConfig();

            new Domain_AmoCRM( mapper);

            new Domain_1C( mapper);

            var action = new User1C(database, null, mapper);


            Contact contact = new Contact();

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

            contact.Tags = new List<Domain.Models.Crm.Fields.Tag> {
                new Domain.Models.Crm.Fields.Tag { Id = 72289, Name = "Заявка с сайта" },
                new Domain.Models.Crm.Fields.Tag { Id = 176263, Name = "callback"}
            };

            contact.Leads = new List<Lead> {
                new Lead { Id = 9982719 },
                new Lead { Id = 10362151 },
                new Lead { Id = 10374575 }
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

            try
            {
                var result = action.Create(contact);
            }
            catch (Exception ex)
            {

            }
        }


        [TestMethod]
        public void ValidResult()
        {
            var ttt = new ValidationResult("ssdfsdfsdf");

            var res = ttt.ToString();
        }
    }
}

