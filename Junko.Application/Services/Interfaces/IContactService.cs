using Junko.Domain.ViewModels.ContactUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Interfaces
{
    public interface IContactService
    {
        #region Methods

        Task CreateContactUs(CreateContactUsDTO contact, string userIp, long? userId);

        #endregion
    }
}
