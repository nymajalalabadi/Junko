using Junko.DataLayer.Context;
using Junko.Domain.Entities.Contacts;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class ContactRepository : IContactRepository
    {
        #region constractor

        private readonly JunkoDbContext _context;

        public ContactRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Contact Us

        public async Task AddContactUs(ContactUs contactUs)
        {
            await _context.ContactUs.AddAsync(contactUs);
        }

        #endregion

        #region Ticket

        public async Task<IQueryable<Ticket>> GetTicketQuery()
        {
            return _context.Tickets
                .Include(t => t.Owner)
                .AsQueryable();
        }

        public async Task<IQueryable<TicketMessage>> GetTicketMessageQuery()
        {
            return _context.TicketMessages.Where(t => !t.IsDelete).AsQueryable();
        }

        public async Task AddTicket(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task AddTicketMessage(TicketMessage ticketMessage)
        {
            await _context.TicketMessages.AddAsync(ticketMessage);
        }

        public void UpdateTicket(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
        }

        public void UpdateTicketMessage(TicketMessage ticketMessage)
        {
            _context.TicketMessages.Update(ticketMessage);
        }

        public async Task<Ticket?> GetTicketById(long ticketId)
        {
            return await _context.Tickets
                .Include(t => t.Owner)
                .FirstOrDefaultAsync(t => t.Id.Equals(ticketId));
        }

        public async Task<TicketMessage?> GetTicketMessageById(long ticketMessageId)
        {
            return await _context.TicketMessages.FirstOrDefaultAsync(t => t.Id.Equals(ticketMessageId));
        }

        public async Task<List<TicketMessage>> GetTicketMessageByTicketId(long ticketId)
        {
            return await _context.TicketMessages
                .Where(t => t.TicketId.Equals(ticketId) && !t.IsDelete)
                .OrderBy(s => s.CreateDate)
                .ToListAsync();
        }

        public void DeleteTicket(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
        }

        public void DeleteTicketMessage(TicketMessage ticket)
        {
            _context.TicketMessages.Remove(ticket);
        }

        #endregion

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
