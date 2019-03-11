using Domain.Models.Crm;
using LibraryAmoCRM.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiLogic.Utils.Mapster
{
    public class WebApiAmoCrm : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ContactDTO, Contact>()
              .Map(dest => dest.Company, src => src.Company)
              .Map(dest => dest.Leads, src => src.Leads.IDs.Select(p => new Lead { Id = p }))
              .Map(dest => dest.Fields, src => src.CustomFields);

            config.NewConfig<Contact, ContactDTO>();

            //.Map(dest => dest.Active, src => src.active == "Активный" ? true : false)
            //.Map(dest => dest.Accepted, src => src.acceptDate)
            //.Map(dest => dest.Type, src => src.typeProgram)
            //.Map(dest => dest.Variant, src => src.viewProgram)
            //.Map(dest => dest.Department, src => src.category)
            //.Map(dest => dest.EducationForm, src => src.formEducation)
            //.Map(dest => dest.Subjects, src => src.listOfSubjects);

            config.NewConfig<CompanyField, Company>();

        }
    }
}
