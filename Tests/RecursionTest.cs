//using LibraryAmoCRM.Models;
//using Mapster;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using WebApi.Infrastructure.Converters;
//using WebApiBusinessLogic.Logics.Listener.DTO;

//namespace WebApiTests
//{
//    [TestClass]
//    public class RecursionTest
//    {
//        [TestMethod]
//        public void URLToJson()
//        {
//            var str = @"leads[status][0][id]=10701093&leads[status][0][name]=Семинар:+Пропитание+в+фитнесе+на+результат&leads[status][0][status_id]=142&leads[status][0][old_status_id]=20067736&leads[status][0][price]=22500&leads[status][0][responsible_user_id]=2079676&leads[status][0][last_modified]=1539782664&leads[status][0][modified_user_id]=2079718&leads[status][0][created_user_id]=2076025&leads[status][0][date_create]=1539782209&leads[status][0][pipeline_id]=9217056&leads[status][0][tags][0][id]=80147&leads[status][0][tags][0][name]=повторное+обращение&leads[status][0][account_id]=17769199&leads[status][0][custom_fields][0][id]=66339&leads[status][0][custom_fields][0][name]=Источник&leads[status][0][custom_fields][0][values][0][value]=Сайт&leads[status][0][custom_fields][0][values][0][enum]=139517&leads[status][0][custom_fields][1][id]=66349&leads[status][0][custom_fields][1][name]=Интересующая+услуга&leads[status][0][custom_fields][1][values][0][value]=Пропитание+в+фитнесе+на+результат&leads[status][0][custom_fields][1][values][0][enum]=153667&leads[status][0][custom_fields][2][id]=72333&leads[status][0][custom_fields][2][name]=Дата+проведения&leads[status][0][custom_fields][2][values][0]=1542067200&leads[status][0][custom_fields][3][id]=497267&leads[status][0][custom_fields][3][name]=Заявка+оплачена&leads[status][0][custom_fields][3][values][0][value]=1&leads[status][0][custom_fields][4][id]=566027&leads[status][0][custom_fields][4][name]=Подтвердил+участие&leads[status][0][custom_fields][4][values][0][value]=1&leads[status][0][custom_fields][5][id]=549619&leads[status][0][custom_fields][5][name]=Отправлено+СМС&leads[status][0][custom_fields][5][values][0][value]=1&leads[status][0][custom_fields][6][id]=566891&leads[status][0][custom_fields][6][name]=Анкета+участия&leads[status][0][custom_fields][6][values][0][value]=https://forms.amocrm.ru/tvzdtx?dp=Q1zaSQHqO%2BhHArUG1UMtpby%2FFHdWZSZgNIHZ%2BvJ%2FRAgB60%2FhsT%2F4Zwi%2F4qY37M3f&leads[status][0][custom_fields][7][id]=570933&leads[status][0][custom_fields][7][name]=Guid-Service&leads[status][0][custom_fields][7][values][0][value]=ab4b290b-9f78-11e6-80e7-0cc47a4b75cc&account[subdomain]=apfitness";

//            var str2 = @"contacts%5Bupdate%5D%5B0%5D%5Bid%5D=22309159&contacts%5Bupdate%5D%5B0%5D%5Bname%5D=%D0%98%D0%B4%D0%B6%D1%8F%D0%BD+%D0%98%D0%BB%D1%8C%D1%8F+%D0%AE%D1%80%D1%8C%D0%B5%D0%B2%D0%B8%D1%87&contacts%5Bupdate%5D%5B0%5D%5Bresponsible_user_id%5D=2079718&contacts%5Bupdate%5D%5B0%5D%5Bdate_create%5D=1529344388&contacts%5Bupdate%5D%5B0%5D%5Blast_modified%5D=1538066975&contacts%5Bupdate%5D%5B0%5D%5Bmodified_user_id%5D=2079718&contacts%5Bupdate%5D%5B0%5D%5Bcompany_name%5D=%D0%90%D0%A1%D0%A1%D0%9E%D0%A6%D0%98%D0%90%D0%A6%D0%98%D0%AF+%D0%9F%D0%A0%D0%9E%D0%A4%D0%95%D0%A1%D0%A1%D0%98%D0%9E%D0%9D%D0%90%D0%9B%D0%9E%D0%92+%D0%A4%D0%98%D0%A2%D0%9D%D0%95%D0%A1%D0%90&contacts%5Bupdate%5D%5B0%5D%5Blinked_company_id%5D=22797025&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B0%5D%5Bid%5D=54667&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B0%5D%5Bname%5D=%D0%A2%D0%B5%D0%BB%D0%B5%D1%84%D0%BE%D0%BD&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B0%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=89031453455&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B0%5D%5Bvalues%5D%5B0%5D%5Benum%5D=114611&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B0%5D%5Bcode%5D=PHONE&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B1%5D%5Bid%5D=54665&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B1%5D%5Bname%5D=%D0%94%D0%BE%D0%BB%D0%B6%D0%BD%D0%BE%D1%81%D1%82%D1%8C&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B1%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=Deveoper&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B1%5D%5Bcode%5D=POSITION&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B2%5D%5Bid%5D=54669&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B2%5D%5Bname%5D=Email&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B2%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=kloder3%40gmail.com&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B2%5D%5Bvalues%5D%5B0%5D%5Benum%5D=114621&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B2%5D%5Bcode%5D=EMAIL&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B3%5D%5Bid%5D=54673&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B3%5D%5Bname%5D=%D0%9C%D0%B3%D0%BD.+%D1%81%D0%BE%D0%BE%D0%B1%D1%89%D0%B5%D0%BD%D0%B8%D1%8F&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B3%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=kloder1&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B3%5D%5Bvalues%5D%5B0%5D%5Benum%5D=114625&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B3%5D%5Bcode%5D=IM&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B4%5D%5Bid%5D=548465&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B4%5D%5Bname%5D=%D0%9E%D1%82%D0%BA%D1%83%D0%B4%D0%B0+%D1%83%D0%B7%D0%BD%D0%B0%D0%BB+%D0%BE+FPA&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B4%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=%D0%9F%D0%BE+%D1%80%D0%B5%D0%BA%D0%BE%D0%BC%D0%B5%D0%BD%D0%B4%D0%B0%D1%86%D0%B8%D0%B8&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B4%5D%5Bvalues%5D%5B0%5D%5Benum%5D=1143911&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B5%5D%5Bid%5D=72337&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B5%5D%5Bname%5D=%D0%93%D0%BE%D1%80%D0%BE%D0%B4&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B5%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=%D0%9C%D0%BE%D1%81%D0%BA%D0%B2%D0%B0&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B6%5D%5Bid%5D=565515&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B6%5D%5Bname%5D=%D0%94%D0%B0%D1%82%D0%B0+%D1%80%D0%BE%D0%B6%D0%B4%D0%B5%D0%BD%D0%B8%D1%8F&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B6%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=02.02.1978&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B7%5D%5Bid%5D=565517&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B7%5D%5Bname%5D=%D0%9E%D0%B1%D1%80%D0%B0%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B7%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=%D0%92%D1%8B%D1%81%D1%88%D0%B5%D0%B5&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B8%5D%5Bid%5D=565519&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B8%5D%5Bname%5D=%D0%9E%D0%BF%D1%8B%D1%82+%D0%B7%D0%B0%D0%BD%D1%8F%D1%82%D0%B8%D1%8F+%D1%81%D0%BF%D0%BE%D1%80%D1%82%D0%BE%D0%BC&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B8%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=3+%D0%B3%D0%BE%D0%B4%D0%B0&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B9%5D%5Bid%5D=565521&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B9%5D%5Bname%5D=%E2%84%96+%D0%BF%D0%BE%D0%B4%D0%B3%D1%80%D1%83%D0%BF%D0%BF%D1%8B+%28%D0%BF%D0%BE+%D0%B6%D0%B5%D0%BB%D0%B0%D0%BD%D0%B8%D1%8E%29&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B9%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=23&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B10%5D%5Bid%5D=565525&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B10%5D%5Bname%5D=%D0%9C%D0%B5%D1%81%D1%82%D0%BE+%D0%B6%D0%B8%D1%82%D0%B5%D0%BB%D1%8C%D1%81%D1%82%D0%B2%D0%B0&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B10%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=Avtozavod&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B11%5D%5Bid%5D=571611&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B11%5D%5Bname%5D=Guid&contacts%5Bupdate%5D%5B0%5D%5Bcustom_fields%5D%5B11%5D%5Bvalues%5D%5B0%5D%5Bvalue%5D=7f31a3bb-b2b8-11e8-8103-0cc47a4b75cc&contacts%5Bupdate%5D%5B0%5D%5Btags%5D%5B0%5D%5Bid%5D=72289&contacts%5Bupdate%5D%5B0%5D%5Btags%5D%5B0%5D%5Bname%5D=%D0%97%D0%B0%D1%8F%D0%B2%D0%BA%D0%B0+%D1%81+%D1%81%D0%B0%D0%B9%D1%82%D0%B0&contacts%5Bupdate%5D%5B0%5D%5Btags%5D%5B1%5D%5Bid%5D=176263&contacts%5Bupdate%5D%5B0%5D%5Btags%5D%5B1%5D%5Bname%5D=callback&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B9982719%5D%5BID%5D=9982719&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B9982767%5D%5BID%5D=9982767&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B9982773%5D%5BID%5D=9982773&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B9990367%5D%5BID%5D=9990367&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B9967531%5D%5BID%5D=9967531&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10060679%5D%5BID%5D=10060679&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10062661%5D%5BID%5D=10062661&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10062733%5D%5BID%5D=10062733&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10063603%5D%5BID%5D=10063603&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10063703%5D%5BID%5D=10063703&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10103287%5D%5BID%5D=10103287&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10106003%5D%5BID%5D=10106003&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10124239%5D%5BID%5D=10124239&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10191357%5D%5BID%5D=10191357&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10224941%5D%5BID%5D=10224941&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10224973%5D%5BID%5D=10224973&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10225387%5D%5BID%5D=10225387&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10225429%5D%5BID%5D=10225429&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10225567%5D%5BID%5D=10225567&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10235219%5D%5BID%5D=10235219&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10235245%5D%5BID%5D=10235245&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10235441%5D%5BID%5D=10235441&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10235461%5D%5BID%5D=10235461&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10238191%5D%5BID%5D=10238191&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10238645%5D%5BID%5D=10238645&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10208297%5D%5BID%5D=10208297&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10291589%5D%5BID%5D=10291589&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10342245%5D%5BID%5D=10342245&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10345597%5D%5BID%5D=10345597&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10362151%5D%5BID%5D=10362151&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10374575%5D%5BID%5D=10374575&contacts%5Bupdate%5D%5B0%5D%5Blinked_leads_id%5D%5B10438403%5D%5BID%5D=10438403&contacts%5Bupdate%5D%5B0%5D%5Btype%5D=contact&account%5Bsubdomain%5D=apfitness";
//            var dff = Uri.UnescapeDataString(str2);

//            var result = new URLParamsToJsonConverter().Covert(dff) as JObject;
//            var view = result.ToString();

//            IList<string> entitiesNames = result.Properties().Select(p => p.Name).ToList();
//            entitiesNames.Remove("account");

//            var Ret = new List<CrmEventDTO>();

//            Dictionary<string, Type> TypeForEvent = new Dictionary<string, Type> {
//                { "contacts", typeof(EventContactModel) }
//            };


//            foreach (var entityName in entitiesNames)
//            {
//                var root = result[entityName] as JObject;

//                var eventsNames = root.Properties().Select(p => p.Name).ToList();

//                foreach (var eventName in eventsNames)
//                {
//                    var item = new CrmEventDTO();
//                    item.Event = eventName;
//                    item.Entities = new List<CrmEventEntityBase>();

//                    var items = result[entityName][eventName] as JArray;

//                    foreach (var i in items)
//                    {
//                        var rr = ((JObject)i).ToObject(TypeForEvent[entityName]);

//                        item.Entities.Add(rr as CrmEventEntityBase);
//                    }

//                    Ret.Add(item);
//                }
                
//            }

//            T fff<T>(object obj) where T : class
//            {
//                return (T)obj;
//            }


//            //var dkkkf = result["leads"]["status"][0];

//            //var oods = dkkkf.ToObject<LeadDTO>();

//            //var sdfss = dkkkf.Adapt<LeadDTO>();

//            Assert.IsInstanceOfType(result, typeof(JObject));
//            Assert.IsNotInstanceOfType(result, typeof(JArray));

//            Assert.IsInstanceOfType(result["leads"]["status"][0]["custom_fields"], typeof(JArray));
//            Assert.AreEqual(result["leads"]["status"][0]["custom_fields"].Count(), 8);
//        }
//    }

//}
