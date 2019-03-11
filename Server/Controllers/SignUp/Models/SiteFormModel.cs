using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers.SignUp.Models
{
    public class SiteFormModel : IValidatableObject
    {
        public SignUp.Models.ContactModel Contact { get; set; }
        public SignUp.Models.LeadModel Lead { get; set; }
        public SignUp.Models.FormModel Form { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            // Validate Contact
            //if (string.IsNullOrWhiteSpace( this.Contact.Name ))
            //    errors.Add( new ValidationResult( "Не указано ФИО" ) );

            var ph = !string.IsNullOrWhiteSpace(this.Contact.Phone);
            var em = !string.IsNullOrWhiteSpace(this.Contact.Email);

            if (ph || em) return errors;

            if (string.IsNullOrWhiteSpace( this.Contact.Phone ))
                errors.Add( new ValidationResult( "Не указан телефон пользователя" ) );
            if (string.IsNullOrWhiteSpace( this.Contact.Email ))
                errors.Add( new ValidationResult( "Не указан email пользователя" ) );

            // Validate Lead
            //if (string.IsNullOrWhiteSpace( this.Lead.Name ))
            //    errors.Add( new ValidationResult( "Не указано название мероприятия" ) );

            return errors;
        }

        
    }


}
