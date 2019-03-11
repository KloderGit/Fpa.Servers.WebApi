using Common.Configuration.Crm;
using Common.Extensions.LeadDomain;
using Domain.Models.Crm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiLogic.Logics.SignUp.Fabric
{
    public class LeadBuilder
    {
        Lead lead;

        public LeadBuilder()
        {
            lead = new Lead();
        }

        public LeadBuilder(Lead lead)
        {
            this.lead = lead;
        }

        public LeadBuilder Name(string type, Dictionary<int, string> titles, string value)
        {
            if (String.IsNullOrEmpty( value )) value = "Название не указано...";

            this.lead.Name = value; ;

            this.lead.Sources(139517);

            var res = titles.FirstOrDefault(x => x.Value.ToUpper().Trim() == value.ToUpper().Trim());

            if (!res.Equals(default(KeyValuePair<int, string>)))
            {
                if (type == "Семинары")
                {
                    this.lead.Seminar(res.Key);
                }
                if (type == "Курсы")
                {
                    this.lead.Program(res.Key);
                }
            }

            this.lead.Fields = this.lead.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.lead.Fields = this.lead.GetChanges().Fields;

            return this;
        }

        public LeadBuilder Price(int? value)
        {
            if (!value.HasValue) return this;

            this.lead.Price = value.Value;

            return this;
        }

        public LeadBuilder Date(DateTime value)
        {
            if (value == default( DateTime )) return this;

            this.lead.SeminarDate( value );
            this.lead.ProgramStartDate( value );
            this.lead.Fields = lead.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.lead.Fields = lead.GetChanges().Fields;
            return this;
        }

        public LeadBuilder Guid(string value)
        {
            if (String.IsNullOrEmpty( value )) return this;

            this.lead.Guid( value );
            this.lead.Fields = this.lead.Fields ?? new List<Domain.Models.Crm.Fields.Field>();
            this.lead.Fields = this.lead.GetChanges().Fields;

            return this;
        }

        public LeadBuilder EducationType(string value)
        {
            if (String.IsNullOrEmpty( value )) value = "";

            EducationTypeEnum manager;
            PipelineStartStatusEnum status;

            try
            {
                manager = (EducationTypeEnum)Enum.Parse( typeof( EducationTypeEnum ), value.ToUpper().Trim() );
                status = (PipelineStartStatusEnum)Enum.Parse( typeof( PipelineStartStatusEnum ), value.ToUpper().Trim() );
            }
            catch (ArgumentException ex)
            {
                manager = EducationTypeEnum.Default;
                status = PipelineStartStatusEnum.Default;
            }
            catch (NullReferenceException ex)
            {
                manager = EducationTypeEnum.Default;
                status = PipelineStartStatusEnum.Default;
            }

            lead.ResponsibleUserId = (int)manager;
            lead.Status = (int)status;

            return this;
        }

        public Lead Get()
        {
            return this.lead;
        }

        public static implicit operator Lead(LeadBuilder builder)
        {
            return builder.Get();
        }
    }
}
