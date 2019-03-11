using Common.Configuration.Crm;
using Common.Extensions;
using Library1C;
using LibraryAmoCRM.Models;
using System;
using System.Linq;

namespace WebApiLogic.Infrastructure.CrmDoEventActions.Shared
{
    public class CreateUser1C
    {
        UnitOfWork service;

        public CreateUser1C(UnitOfWork service)
        {
            this.service = service;
        }

        public string Create(ContactDTO user)
        {
            string guid;

            try
            {
                guid = service.Persons.Add(
                 FIO: user.Name,
                 Phone: user.CustomFields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone).Values.FirstOrDefault().Value,
                 Email: user.CustomFields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email).Values.FirstOrDefault().Value,
                 BirthDay: user.CustomFields.FirstOrDefault(x => x.Id == 565515).Values.FirstOrDefault().Value.ToDateTime(),
                 City: user.CustomFields.FirstOrDefault(x => x.Id == 72337).Values.FirstOrDefault().Value,
                 Position: user.CustomFields.FirstOrDefault(x => x.Id == 54665).Values.FirstOrDefault().Value,
                 Education: user.CustomFields.FirstOrDefault(x => x.Id == 565517).Values.FirstOrDefault().Value,
                 Expirience: user.CustomFields.FirstOrDefault(x => x.Id == 565519).Values.FirstOrDefault().Value,
                 Address: user.CustomFields.FirstOrDefault(x => x.Id == 565525).Values.FirstOrDefault().Value
                ).Result.GUID;
            }
            catch (Exception ex)
            {
                throw new FormatException(ex.Message);
            }
            
            return guid;
        }
    }
}
