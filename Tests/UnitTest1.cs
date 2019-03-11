using Common.Extensions.ContactDomain;
using Common.Extensions.LeadDomain;
using Domain.Models.Crm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiLogic.Infrastructure.Helpers;

namespace WebApiTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ImplicitBuilder()
        {
            var builder = new FormDTOBuilder();

            builder.EducationType("ОТКРЫТОЕ");

            Assert.IsInstanceOfType((Contact)builder, typeof(Contact));
            Assert.IsInstanceOfType((Lead)builder, typeof(Lead));
        }

        [TestMethod]
        public void SetResponsibleUser()
        {
            var builder = new FormDTOBuilder();

            builder.EducationType("");

            Assert.AreEqual(((Contact)builder).ResponsibleUserId, 2079679);

            builder.EducationType(null);

            Assert.AreEqual(((Contact)builder).ResponsibleUserId, 2079679);

            builder.EducationType("ОТКРЫТОЕ");

            Assert.AreEqual(((Lead)builder).ResponsibleUserId, 2079682);
        }

        [TestMethod]
        public void ContactName()
        {
            var builder = new FormDTOBuilder();

            builder.ContactName("Илья Иджян");

            builder.ContactName("Илья");

            Assert.AreEqual(((Contact)builder).Name, "Илья Иджян");

            builder.ContactName("Илья Иджян Юрьевич");

            Assert.AreEqual(((Contact)builder).Name, "Илья Иджян Юрьевич");
        }

        [TestMethod]
        public void LeadName()
        {
            var builder = new FormDTOBuilder();

            builder.ContactName("Сделка %");

            builder.ContactName("ООО");

            Assert.AreEqual(((Contact)builder).Name, "Сделка %");

            builder.ContactName("Программа обучения");

            Assert.AreEqual(((Contact)builder).Name, "Программа обучения");
        }

        [TestMethod]
        public void CustomFields()
        {
            var builder = new FormDTOBuilder();

            builder.Phone("89991453412");

            Assert.AreEqual(((Contact)builder).Phones().FirstOrDefault().Value, "89991453412");


            builder.Email("kldoder");

            Assert.AreEqual(((Contact)builder).Email().FirstOrDefault().Value, "kldoder");

            builder.City("Москва");

            Assert.AreEqual(((Contact)builder).City(), "Москва");

            builder.DateOfEvent(new DateTime(2000, 12, 05));

            Assert.AreEqual(((Lead)builder).SeminarDate(), new DateTime(2000, 12, 05));
        }

        [TestMethod]
        public void Fields()
        {
            var builder = new FormDTOBuilder();

            Assert.AreEqual(((Lead)builder).Price, null);

            builder.Price(456654);
            Assert.AreEqual(((Lead)builder).Price, 456654);
        }

        [TestMethod]
        public void AllFieldsAndContact()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(123, "Подвесной трениенг");

            var item = new Contact();
            item.Name = "Илья";
            item.City("Moscow");
            item.Phones(LibraryAmoCRM.Configuration.PhoneTypeEnum.MOB, "89991453412");
            item.Email(LibraryAmoCRM.Configuration.EmailTypeEnum.PRIV, "dirkld@yandex.ru");

            var builder = new FormDTOBuilder(item);

            builder.EducationType("Дистанционное");
            builder.ContactName("Иджян Илья");
            builder.LeadName(dict, "Семинары", "Подвесной трениенг");
            builder.Email("dirkld@yandex.ru" );
            builder.Price(150000);
            builder.DateOfEvent(new DateTime(2018, 10, 11));

            Contact contact = builder;
            Lead lead = builder;

            Assert.AreSame(item, contact);
            Assert.AreEqual(contact.City(), "Moscow");
        }


        [TestMethod]
        public void ParseString()
        {
            var dict = GetString().Split( '&' ).ToList().Where(x=>x.StartsWith( "leads" ) ).FirstOrDefault();

            var oneElement = dict.Split( '=' ).ToArray();

            var element = new Dictionary<string, string>() {
                { oneElement[0], oneElement[1] }
            };

            var obj = new JObject();

            var arr = element.FirstOrDefault().Key.Split( "[" ).Select( x => x.Replace( "]", "" ) ).ToList();

            String access_token = HttpUtility.ParseQueryString( dict ).Get( "leads" );

            ;

            string GetString()
            {
                return @"leads[update][0][id]=10281843&leads[update][0][name]=Программа:+Фитнес-нутрициолог&leads[update][0][status_id]=18855169&leads[update][0][old_status_id]=18903799&leads[update][0][price]=35000&leads[update][0][responsible_user_id]=2267437&leads[update][0][last_modified]=1540893721&leads[update][0][modified_user_id]=0&leads[update][0][created_user_id]=2076025&leads[update][0][date_create]=1537110931&leads[update][0][pipeline_id]=1042456&leads[update][0][account_id]=17769199&leads[update][0][custom_fields][0][id]=66339&leads[update][0][custom_fields][0][name]=Источник&leads[update][0][custom_fields][0][values][0][value]=Сайт&leads[update][0][custom_fields][0][values][0][enum]=139517&leads[update][0][custom_fields][1][id]=72417&leads[update][0][custom_fields][1][name]=Начало+программы&leads[update][0][custom_fields][1][values][0]=1537920000&leads[update][0][custom_fields][2][id]=72419&leads[update][0][custom_fields][2][name]=Конец+договора&leads[update][0][custom_fields][2][values][0]=1541980800&leads[update][0][custom_fields][3][id]=227457&leads[update][0][custom_fields][3][name]=Программа+обучения&leads[update][0][custom_fields][3][values][0][value]=Фитнес-нутрициолог+(дистант)&leads[update][0][custom_fields][3][values][0][enum]=489759&leads[update][0][custom_fields][4][id]=554029&leads[update][0][custom_fields][4][name]=Группа+обучения+[дистант]&leads[update][0][custom_fields][4][values][0][value]=ФН-4&leads[update][0][custom_fields][4][values][0][enum]=1150609&leads[update][0][custom_fields][5][id]=566897&leads[update][0][custom_fields][5][name]=Тип&leads[update][0][custom_fields][5][values][0][value]=Дистанционное&leads[update][0][custom_fields][5][values][0][enum]=1165857&leads[update][0][custom_fields][6][id]=570933&leads[update][0][custom_fields][6][name]=Guid-Service&leads[update][0][custom_fields][6][values][0][value]=ca8e5a64-87dc-11e7-80f4-0cc47a4b75cc&account[subdomain]=apfitness&account[id]=17769199&account[_links][self]=https://apfitness.amocrm.ru";
            }

        }
    }
}
