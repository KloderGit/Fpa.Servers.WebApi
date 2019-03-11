using Common.BusinessLogicHelpers.Crm.Actions;
using Common.Configuration.Crm;
using Common.Mapping;
using Domain.Models.Crm;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiLogic.Logics.CallBack.Models;

namespace WebApiLogic.Logics.CallBack
{
    public class CallBackLogic
    {
        TypeAdapterConfig mapper;
        IDataManager crm;
        ILogger logger;

        public CallBackLogic(TypeAdapterConfig mapper, IDataManager crm, ILoggerFactory loggerFactory)
        {
            this.mapper = mapper;
                new RegisterCommonMaps(mapper);
            this.crm = crm;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }

        public async Task<bool> CreateRecallTask(CallBackDTO model)
        {
            Contact contact;

            try
            {
                // Check available contact
                var action = new FindContactActions(crm, logger);
                var result = await action.LookForContact(phone: model.UserPhone);
                contact = result?.Adapt<Contact>(mapper);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при поиске контакта в Crm");
                return false;
            }

            // Create contact
            if (contact == null)
            {
                var buildContact = new Common.BusinessLogicHelpers.Crm.Builders.ContactBuilder();
                buildContact.Name(model.UserName);
                buildContact.Phone(model.UserPhone);
                contact = (Contact)buildContact;

                contact.ResponsibleUserId = GetManager(model);

                var contactDTO = contact.Adapt<ContactDTO>(mapper);

                try
                {
                    System.Threading.Tasks.Task taskPrepareContactLog = System.Threading.Tasks.Task.Factory.StartNew(
                       () => logger.LogInformation("Подготовлен контакт для добавления - {@Contact}", contact)
                    );

                    var query = crm.Contacts.Add(contactDTO).Result;
                    contact = query.Adapt<Contact>(mapper);

                    System.Threading.Tasks.Task taskreateContactLog = System.Threading.Tasks.Task.Factory.StartNew(
                        () => logger.LogInformation("Создан контакт {Name} | {Id}", contactDTO.Name, contact.Id)
                    );
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Ошибка создания контакта с сайта");
                    return false;
                }
            }

            if (contact != null)
            {
                var manager = GetManager(model);

                var modelTask = new TaskDTO()
                {
                    ElementId = contact.Id,
                    ElementType = (int)ElementTypeEnum.Контакт,
                    Text = "Клиент заказал обратный звонок!" + "\r\n"
                        + "Со страницы мероприятия- " + model.ProgramTitle + "\r\n" +
                        "URL.: " + model.Url,
                    ResponsibleUserId = manager,
                    CompleteTillAt = DateTime.Now
                };

                logger.LogInformation("Подготовлена задача - {Task}", modelTask);

                try
                {
                           var tsk = crm.Tasks.Add(new[] { modelTask }).Result;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Ошибка создания задачи для контакта - " + contact.Id);
                    return false;
                }
            }

            return true;
        }

        private int GetManager(CallBackDTO model)
        {
            int manager = (int)ResponsibleUserEnum.Анастасия_Столовая;

            if (model.ProgramType == null || model.Url == null) return manager;

            var uri = new Uri(model.Url);

            if (uri.Segments.Any(i => i.ToUpper() == "kursy-fitnesa/".ToUpper()))
            {
                if (uri.Segments.Any(i => i.ToUpper() == "seminary/".ToUpper()))
                {
                    manager = model.ProgramType.ToUpper() == "КОРПОРАТИВНАЯ".ToUpper() ? (int)ResponsibleUserEnum.Лина_Серрие : (int)ResponsibleUserEnum.Ирина_Моисеева;
                }
                else
                {
                    manager = model.ProgramType.ToUpper() == "ДИСТАНЦИОННАЯ".ToUpper() ? (int)ResponsibleUserEnum.Филатова_Елена : (int)ResponsibleUserEnum.Анастасия_Столовая;
                }
            }

            if (uri.Segments.Any(i => i.ToUpper() == "shkola-fitnesa/".ToUpper()))
            {
                manager = (int)ResponsibleUserEnum.Анна_Скорюпина;
            }

            return manager;
        }
    }
}
