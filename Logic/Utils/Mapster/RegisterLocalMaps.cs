using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Domain.Models.Education;
using LibraryAmoCRM.Configuration;
using LibraryAmoCRM.Models;
using LibraryAmoCRM.Models.Fields;
using Mapster;
using ServiceLibraryNeoClient.Models;
using ServiceReference1C;
using System;
using System.Collections.Generic;
using System.Linq;
using WebPortalBuisenessLogic.Models.Crm;
using WebPortalBuisenessLogic.Models.DataBase;

namespace WebPortalBuisenessLogic.Utils.Mapster
{
    public class RegisterLocalMaps
    {
        public RegisterLocalMaps(TypeAdapterConfig mapper)
        {
            mapper.NewConfig<ProgramNode, ProgramDTO>()
                .Map(dest => dest.Form, src => src.Form.Title)
                .Map(dest => dest.Department, src => src.Department.Title)
                .Map(dest => dest.Subjects, src => src.Subjects.Select(i => i.Title));

            // ----------------------------

            mapper.NewConfig<WizardDTO, Lead>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Id, src => src.LeadId);

            mapper.NewConfig<WizardDTO, Contact>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Id, src => src.ContactId);

            mapper.NewConfig<Lead, WizardDTO>()
                .IgnoreNullValues(true)
                .Map(dest => dest.LeadId, src => src.Id)
                .Map(dest => dest.Program, src => src.Program() != null ? src.Program().Enum: null)

                .Map(dest => dest.Birthday, src => src.MainContact.Birthday())
                .Map(dest => dest.City, src => src.MainContact.City())
                .Map(dest => dest.ContactId, src => src.MainContact.Id)
                .Map(dest => dest.Education, src => src.MainContact.Education())

                  .IgnoreIf((src, dest) => src.MainContact.Email() == null, dest => dest.Email)
                  .Map(dest => dest.Email, src => src.MainContact.Email().FirstOrDefault().Value)

                .Map(dest => dest.Expirience, src => src.MainContact.Experience())
                .Map(dest => dest.Name, src => src.MainContact.Name)

                  .IgnoreIf((src, dest) => src.MainContact.Phones() == null, dest => dest.Phone)
                  .Map(dest => dest.Phone, src => src.MainContact.Phones().FirstOrDefault().Value)

                .Map(dest => dest.ProgramPart, src => src.MainContact.GroupPart())
                .Map(dest => dest.Subway, src => src.MainContact.Location());

        }

        public object src { get; }
    }
}
