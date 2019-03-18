using Common.Extensions;
using Common.Extensions.ContactDomain;
using Domain.Models.Crm;
using Domain.Models.Crm.Parent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public class ClearPhone<T> : IDoAction<T> where T : Contact
    {
        public async Task<T> DoActionAsync(T item)
        {
            foreach (var phone in item.Phones() ?? new Dictionary<LibraryAmoCRM.Configuration.PhoneTypeEnum, string>())
            {
                if (phone.Value != phone.Value.LeaveJustDigits())
                {
                    item.Phones(phone.Key, phone.Value.LeaveJustDigits());
                }
            }
            return item;
        }
    }
}
