using Common.DTO.Service1C;
using Common.Extensions.ContactDomain;
using Common.Extensions.LeadDomain;
using Domain.Models.Crm.Parent;
using Library1C;
using Library1C.DTO;
using LibraryAmoCRM.Infarstructure.QueryParams;
using LibraryAmoCRM.Interfaces;
using LibraryAmoCRM.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApiLogic.Logics.Listener.DTO;
using WebApiLogic.Logics.Listener.Models;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public class SendLeadTo1C : HandlerBase<Lead>
    {
        UnitOfWork database;
        ILoggerFactory loggerFactory;

        public SendLeadTo1C(IDataManager crm, TypeAdapterConfig mapper, ILoggerFactory loggerFactory, UnitOfWork service1C)
            :base (crm, mapper, loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.database = service1C;
        }

        public override async void DoActionAsync(object sender, EventDTO events)
        {
            if ( IsPassed(events) != true ) return;

            var toUpdate = events.Entities ?? new List<EntityCore>();

            foreach (var itemCore in toUpdate)
            {
                var lead = itemCore as Lead;
                var contact = await PrepareContactAsync(lead);
                if (lead == null || contact == null) break;

                var result = await SendLead(GetModelFor1C(lead, contact));
            }
        }

        private async Task<Contact> PrepareContactAsync(Lead item)
        {
            Lead lead = null;
            Contact contact = null;

            try
            {
                string leadGuid = String.Empty;
                string contactGuid = String.Empty;

                if ( String.IsNullOrEmpty(item.Guid()) ) throw new NullReferenceException();

                lead = await GetLeadAsync(item.Id);
                    if (lead.MainContact == null || lead.MainContact.Id == default(int)) throw new NullReferenceException();

                contact = await GetContactAsync(lead.MainContact.Id);
                    if (contact == null) throw new NullReferenceException();


                if (String.IsNullOrEmpty(contact.Guid()))
                {
                    contactGuid = await contact.FindIn1C(database);

                    if (String.IsNullOrEmpty(contactGuid)) contactGuid = await contact.CreateIn1C(database, mapper);
                    if (!String.IsNullOrEmpty(contactGuid))
                    { contact.SetGuid(contactGuid); contact.Guid(contactGuid); await crm.Contacts.Update(contact.GetChanges().Adapt<ContactDTO>(mapper)); }
                }
            }
            catch (NullReferenceException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return contact;
        }

        private async Task<Lead> GetLeadAsync(int id)
        {
            var queryLead = await crm.Leads.Get().Filter(i => i.Id = id).Execute();
            if (queryLead == null) throw new NullReferenceException();
            return queryLead.FirstOrDefault().Adapt<Lead>(mapper);
        }

        private async Task<Contact> GetContactAsync(int id)
        {
            var queryContact = await crm.Contacts.Get().Filter(i => i.Id = id).Execute();
            if (queryContact == null) throw new NullReferenceException();
            return queryContact.FirstOrDefault().Adapt<Contact>(mapper);
        }

        private SendLeadto1CDTo GetModelFor1C(Lead lead, Contact contact)
        {
            var dto = new SendLeadto1CDTo();
            dto.ProgramGuid = lead.Guid();
            dto.UserGuid = contact.Guid();
            dto.ContractPrice = lead.Price.Value;
            dto.ContractTitle = contact.Name;
            dto.DecreeTitle = contact.Name;

            if (lead.Pipeline.Id == 917056 || lead.Pipeline.Id == 920008 || lead.Pipeline.Id == 1102975)
            {
                dto.ContractEducationStart = lead.SeminarDate().Value;
                dto.ContractEducationEnd = lead.SeminarDate().Value.AddDays(3);
                dto.ContractExpire = lead.SeminarDate().Value.AddDays(3);
                dto.ContractGroup = lead.SeminarDate().Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            if (lead.Pipeline.Id == 920011 || lead.Pipeline.Id == 1042456)
            {
                dto.ContractEducationStart = lead.ProgramStartDate().Value;
                dto.ContractEducationEnd = lead.ProgramStartDate().Value.AddDays(180);
                dto.ContractExpire = lead.ContractExpireDate().Value;

                dto.ContractGroup = "Общая группа";

                if (lead.Pipeline.Id == 1042456) dto.ContractGroup = lead.DistantGroup().Value;
                if (lead.Pipeline.Id == 920011) dto.ContractGroup = lead.FullTimeGroup().Value;

                dto.ContractSubGroup = lead.SubGroup()?.Value ?? "";
            }

            return dto;
        }

        private async Task<string> SendLead(SendLeadto1CDTo dto)
        {
            if (dto.isValid == false) throw new ArgumentException(String.Join(" | ", dto.GetValidateErrors()));

            var result = await database.Persons.InviteTo1C(dto.Adapt<AddLeadDTO>(mapper));

            if (result != "Студент зачислен") throw new ArgumentException();

            return String.IsNullOrEmpty(result) ? null : result;
        }
    }
}
