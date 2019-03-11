using Common.Configuration.Crm;
using Common.Interfaces;
using Common.Mapping;
using Library1C;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApiLogic.Infrastructure.Actions;
using WebApiLogic.Infrastructure.CrmDoEventActions;
using WebApiLogic.Models.Crm;

namespace WebApiLogic
{
    public class BusinessLogic
    {
        ILoggerService logger;
        ILoggerFactory loggerFactory;
        ILogger currentLogger;
        TypeAdapterConfig mapper;

        UnitOfWork database;
        ServiceLibraryNeoClient.Implements.DataManager neodatabase;

        CrmEventTypes eventsType = new CrmEventTypes();

        UpdateGuid updGuid;
        UpdatePhone updPhone;
        SendLeadTo1CEvent sendLead;

        protected IDataManager crm;

        public BusinessLogic(IDataManager amocrm)
        {
            this.crm = amocrm;
        }

        public BusinessLogic(
            ILoggerService logger, 
            ILoggerFactory loggerFactory,
            IConfiguration configuration, 
            TypeAdapterConfig mapping, 
            UnitOfWork service1C,
            //ServiceLibraryNeoClient.Implements.DataManager neo, 
            Lazy<ServiceLibraryNeoClient.Implements.DataManager> neo,
            IDataManager amocrm)
        {
            this.logger = logger;   // Логи
            this.loggerFactory = loggerFactory;
            this.currentLogger = loggerFactory.CreateLogger(this.ToString());

            this.mapper = mapping;  // Maps
                new RegisterCommonMaps(mapper);

            this.crm = amocrm; // Amo
            this.database = service1C;  // 1C

            neodatabase = neo.Value;  // neo

            // Events
            updGuid = new UpdateGuid(crm, database, eventsType, mapper, loggerFactory);
            updPhone = new UpdatePhone(crm, eventsType, mapper, loggerFactory);
            sendLead = new SendLeadTo1CEvent(crm, database, eventsType, mapper, loggerFactory);

            //new RegisterMapsterConfig();

        }

        public void GetEvent(CrmEvent item)
        {
            foreach (var evnt in item.Events)
            {
                switch (evnt.Event)
                {
                    case "add":
                        eventsType.OnAdd(item);      // !!!!!
                        break;
                    case "update":
                        eventsType.OnUpdate(item);
                        break;
                    case "ChangeResponsibleUser":
                        eventsType.OnResponsible(item);
                        break;
                    case "delete":
                        eventsType.OnDelete(item);
                        break;
                    case "note":
                        eventsType.OnNote(item);
                        break;
                    case "status":
                        eventsType.OnStatus(item);
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
            }
        }


        public IEnumerable<ProgramList> GetProgramsListForAmo()
        {
            var programs = neodatabase.Programs.GetList().Where( x => x.Active );

            var list = programs.Select( it => new ProgramList { Title = it.Title, Guid = it.Guid, Type = it.Type, Department = it.Department.Title } );

            return list;
        }


        public async Task<IEnumerable<LeadDTO>> GetLeadsByStatus(int status)
        {
            var sd = new LeadDTO();
            sd.ResponsibleUserId = (int)ResponsibleUserEnum.Анастасия_Столовая;

            return await crm.Leads.Get().Filter(x => x.Status = status).Execute();
        }

        private MethodInfo GetMetod(Type genericType)
        {
            return this.GetType().GetMethod("MetodGet").MakeGenericMethod(genericType);
        }

        //private Func<IAcceptParams> MetodGet<T>() where T : CoreDTO
        //{
        //    var result = amocrm.Repository<T>();
        //    return result.Get;
        //}

        //private Action<T> MetodUpdate<T>() where T : CoreDTO
        //{
        //    var result = amocrm.Repository<T>();
        //    return result.Update;
        //}



        public  bool LookForAmoUser(IEnumerable<string> phone, string email)
        {
            //var user = amocrm.Contacts.Get().SetParam(x => x.Phone = phone.LeaveJustDigits()).Execute().Result;
            //var user2 = amocrm.Contacts.Get().SetParam(x => x.Query = email.ClearEmail()).Execute().Result;

            var userAction = new AmoCRMCommonActions(crm, mapper, currentLogger);

            var contactGuid = userAction.FindContact(phone).Result;

            var lead = crm.Leads.Get().Filter(p => p.Query = contactGuid.Name).Execute().Result;

            return true;
        }


        //public async Task<int> CreateLeadFormSite(SignUpForEvent item)
        //{
        //    // Prepare
        //    Contact contact = null;
        //    Lead lead = null;
        //    FormDTOBuilder builder = new FormDTOBuilder(contact, lead);

        //    //var prgms = await amocrm.Catalogs.Get().SetParam(i => i.Catalog = Catalogs.Programs).Execute();

        //    //var program = prgms?.FirstOrDefault(el => el.CustomFields.FirstOrDefault(id=>id.Id == 268359).Values.FirstOrDefault().Value == item.LeadGuid);

        //    var types1 = crm.Account.Embedded.CustomFields.Leads[66349].Enums;
        //    var types2 = crm.Account.Embedded.CustomFields.Leads[227457].Enums;
        //    var types = types2.Union(types1).ToDictionary(pair => pair.Key, pair => pair.Value);
        //    try
        //    {
        //        builder.ContactName(item.ContactName)
        //            .Phone(item.ContactPhones)
        //            .Email(item.ContactEmails)
        //            .City(item.ContactCity)
        //            .LeadName(types, item.EventType, item.LeadName)
        //            .EducationType(item.LeadType)
        //            .Price(item.LeadPrice)
        //            .DateOfEvent(item.LeadDate)
        //            .LeadGuid(item.LeadGuid);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex, "Ошибка при преобразовании запроса с сайта в Модели BL. Данные запроса {@Form}", item);
        //        throw new ArgumentException();
        //    }


        //    // LookFor or Create Contact
        //    var userAction = new UserAmoCRM(crm, mapper, logger);
        //    contact = userAction.FindContact(item.ContactPhones).Result;
        //    if (contact == null) contact = userAction.FindContact(item.ContactEmails).Result;

        //    if (contact == null)
        //    {
        //        try
        //        {
        //            Contact cont = builder;
        //            var queryCreateContact = await crm.Contacts.Add(cont.Adapt<ContactDTO>(mapper));
        //            var queryGetContact = await crm.Contacts.Get().Filter(i => i.Id = queryCreateContact.Id.Value).Execute();
        //            contact = queryGetContact.FirstOrDefault().Adapt<Contact>(mapper);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ArgumentNullException();
        //        }
        //    }

        //    // LookFor Lead
        //    var query = await crm.Leads.Get().Filter(p => p.Query = contact.Name).Execute();
        //    var result = query?.Adapt<IEnumerable<Lead>>(mapper);
        //    var userLeads = result?.Where(l => l.MainContact.Id == contact.Id);
        //    lead = userLeads?.FirstOrDefault(l => l.Name.ToUpper().Trim().Contains(item.LeadName.ToUpper().Trim()));

        //    // Create Lead
        //    ((Lead)builder).Contacts = new List<Contact> { new Contact { Id = contact.Id } };
        //    var queryCreateLead = await crm.Leads.Add(((Lead)builder).Adapt<LeadDTO>(mapper));

        //    var note = new NoteDTO()
        //    {
        //        ElementId = queryCreateLead.Id,
        //        ElementType = (int)ElementTypeEnum.Сделка,
        //        NoteType = 25,
        //        Params = new NoteParams
        //        {
        //            Text = "Адрес отправки запроса: " + item.RequestUrl,
        //            Service = "WebApi | "
        //        }
        //    };

        //    var queryCreateNOte = await crm.Notes.Add(note);

        //    // Add Task
        //    //if (lead != null)
        //    //{
        //    //    var task = new TaskDTO()
        //    //    {
        //    //        ElementId = queryCreateLead.Id,
        //    //        ElementType = (int)ElementTypeEnum.Сделка,
        //    //        CompleteTillAt = DateTime.Today,
        //    //        TaskType = 965749,
        //    //        Text = @"Существует похожая заявка у этого пользователя. Проверить на дубли."
        //    //    };

        //    //    var queryCreateTask = await amocrm.Tasks.Add(task);
        //    //}

        //    return queryCreateLead.Id.Value;
        //}

    }



        
}
