using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.SignUp.Models;

namespace WebApi.Infrastructure.LocalMaps
{
    public class Map_FormToModel
    {
        public Map_FormToModel(TypeAdapterConfig config)
        {
            config.NewConfig<IEnumerable<SiteFormField>, SiteFormModel>()
                .IgnoreNullValues( true )
                .Map(
                    dest => dest.Contact,
                    src => src
                )
                .Map(
                    dest => dest.Lead,
                    src => src
                );

            config.NewConfig<IEnumerable<SiteFormField>, ContactModel>()
                .IgnoreNullValues( true )
                    .Map( 
                        dest => dest.Name, 
                        src => src.FirstOrDefault( x => x.Name.Trim() == "DATA[NAME]" ) != null
                            ? src.FirstOrDefault( x => x.Name.Trim() == "DATA[NAME]" ).Value
                            : null
                    )
                    .Map(
                        dest => dest.Phone,
                        src => src.FirstOrDefault( x => x.Name.Trim() == "DATA[PHONE][]" ) != null 
                            ? src.FirstOrDefault( x => x.Name.Trim() == "DATA[PHONE][]" ).Value 
                            : null
                    )
                    .Map(
                        dest => dest.Email,
                        src => src.FirstOrDefault( x => x.Name.Trim() == "DATA[EMAIL][]" ) != null 
                            ? src.FirstOrDefault( x => x.Name.Trim() == "DATA[EMAIL][]" ).Value 
                            : null
                    )
                    .Map(
                        dest => dest.City,
                        src => src.FirstOrDefault( x => x.Name == "DATA[CITY]" ) != null 
                            ? src.FirstOrDefault( x => x.Name == "DATA[CITY]" ).Value
                            : null
                    );

            config.NewConfig<IEnumerable<SiteFormField>, LeadModel>()
                .IgnoreNullValues( true )
                .Map(
                    dest => dest.Name,
                    src => src.FirstOrDefault( x => x.Name == "DATA[EDU_NAME]" ) != null
                        ? src.FirstOrDefault( x => x.Name == "DATA[EDU_NAME]" ).Value
                        : null
                )
                .Map(
                    dest => dest.Date,
                    src => src.FirstOrDefault( x => x.Name == "DATA[DATE]" ) != null
                        ? DateTime.Parse( src.FirstOrDefault( x => x.Name == "DATA[DATE]").Value, new CultureInfo( "ru-RU" ) ) 
                        : default(DateTime)
                )
                .Map(
                    dest => dest.EducationForm,
                    src => src.FirstOrDefault( x => x.Name == "DATA[EDU_TYPE]" ) != null
                        ? src.FirstOrDefault( x => x.Name == "DATA[EDU_TYPE]" ).Value
                        : null
                )
                .Map(
                    dest => dest.EducationType,
                    src => src.FirstOrDefault( x => x.Name == "TYPE" ) != null
                        ? src.FirstOrDefault( x => x.Name == "TYPE" ).Value
                        : null
                )
                .Map(
                    dest => dest.Agree,
                    src => src.FirstOrDefault( x => x.Name == "DATA[AGREE]" ) != null
                        ? src.FirstOrDefault( x => x.Name == "DATA[AGREE]" ).Value == "1" ? true : false
                        : default(bool)
                )
                .Map(
                    dest => dest.Guid,
                    src => src.FirstOrDefault( x => x.Name == "GUID_EVENT" ) != null
                        ? src.FirstOrDefault( x => x.Name == "GUID_EVENT" ).Value
                        : null
                )
                .Map(
                    dest => dest.isPerson,
                    src => src.FirstOrDefault( x => x.Name == "DATA[SUBJECT]" ) != null
                        ? src.FirstOrDefault( x => x.Name == "DATA[SUBJECT]" ).Value == "F"
                        : default(bool?)
                )
                .Map(
                    dest => dest.Pay,
                    src => src.FirstOrDefault( x => x.Name == "pay" ) != null
                        ? src.FirstOrDefault( x => x.Name == "pay" ).Value
                        : null
                )

                .Map(
                    dest => dest.Price,
                    src => src.FirstOrDefault( x => x.Name == "DATA[PRICE]" ) != null && !String.IsNullOrEmpty(src.FirstOrDefault( x => x.Name == "DATA[PRICE]" ).Value)
                        ? int.Parse( src.FirstOrDefault( x => x.Name == "DATA[PRICE]" ).Value )
                        : default( int )
                );
        }
    }
}
