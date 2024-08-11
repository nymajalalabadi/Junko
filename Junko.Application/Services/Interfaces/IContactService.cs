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

        #region ContactUs

        Task CreateContactUs(CreateContactUsDTO contact, string userIp, long? userId);

        #endregion

        #region Ticket

        Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, long userId);

        Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter);

        #endregion

        #endregion
    }
}
