using Junko.Domain.Entities.Contacts;
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

        Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter);

        Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, long userId);

        Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId);

        #endregion

        #endregion
    }
}
