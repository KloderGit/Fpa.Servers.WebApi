using Domain.Interfaces;
using Domain.Models.Crm.Fields;
using Domain.Models.Crm.Parent;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.Listener.Models;
using WebApiLogic.Logics.Listener.DTO;
using WebApiLogic.Logics.Listener.Models;

namespace WebApi.Controllers.Listener.LocalMaps
{
    public class IncomingEventToDTO
    {
        TypeAdapterConfig config;

        public IncomingEventToDTO(TypeAdapterConfig config)
        {
            this.config = config;

            config.NewConfig<EventViewModel, EventDTO>();

            config.NewConfig<EntityBase, EntityCore>()
                .ConstructUsing(src => GetByType(src));

            config.NewConfig<ContactViewModel, Contact>()
                .IgnoreNullValues(true)
                                //.Map(desc => desc.Leads, src => src.LinkedLeads != null && src.LinkedLeads.Count() > 0 ? src.LinkedLeads.Select( l=> new Lead { Id = l.Id }) : null)
                                .Map(desc => desc.Leads, src => src.LinkedLeads)
                .Map(desc => desc.Fields, src => src.CustomFields);

            config.NewConfig<LeadViewModel, Lead>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Pipeline, src => src.PipelineId == null? null : new Pipeline { Id = src.PipelineId })
                .Map(desc => desc.Fields, src => src.CustomFields);

            config.NewConfig<CustomFieldViewModel, Field>();

            config.NewConfig<FieldViewModel, FieldValue>();

            config.NewConfig<TagViewModel, Tag>();
        }


        private EntityCore GetByType(object src)
        {
            EntityCore res = new EntityCore();

            if (src.GetType() == typeof(LeadViewModel))
            {
                res = src.Adapt<Lead>(config);
            }

            if (src.GetType() == typeof(ContactViewModel))
            {
                res = src.Adapt<Contact>(config);
            }

            return res;
        }

    }
}
