using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface IContactRepository
    {
        #region Methods

        #region ContactUs

        Task AddContactUs(ContactUs contactUs);

        #endregion

        #region Ticket

        Task<IQueryable<Ticket>> GetTicketQuery();

        Task<IQueryable<TicketMessage>> GetTicketMessageQuery();

        Task AddTicket(Ticket ticket);

        Task AddTicketMessage(TicketMessage ticketMessage);

        void UpdateTicket(Ticket ticket);

        void UpdateTicketMessage(TicketMessage ticketMessage);

        Task<Ticket?> GetTicketById(long ticketId);

        Task<TicketMessage?> GetTicketMessageById(long ticketMessageId);

        Task<List<TicketMessage>> GetTicketMessageByTicketId(long ticketId);

        void DeleteTicket(Ticket ticket);

        void DeleteTicketMessage(TicketMessage ticket);

        #endregion

        Task SaveChanges();

        #endregion
    }
}
