using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.Contacts;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.ContactUs;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace Junko.Application.Services.Implementations
{
    public class ContactService : IContactService
    {
        #region consractor

        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        #endregion

        #region Methods

        #region ContactUs

        public async Task CreateContactUs(CreateContactUsDTO contact, string userIp, long? userId)
        {
            var newContact = new ContactUs
            {
                UserId = userId != null && userId.Value != 0 ? userId.Value : (long?)null,
                Subject = contact.Subject,
                Email = contact.Email,
                UserIp = userIp,
                Text = contact.Text,
                FullName = contact.FullName
            };

            await _contactRepository.AddContactUs(newContact);
            await _contactRepository.SaveChanges();
        }

        #endregion

        #region Ticket

        public async Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter)
        {
            var query = await _contactRepository.GetTicketQuery();

            #region filter

            if (filter.UserId != null && filter.UserId != 0)
            {
                query = query.Where(s => s.OwnerId.Equals(filter.UserId));
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => s.Title.Contains(filter.Title));
            }

            if (filter.TicketSection != null)
            {
                query = query.Where(s => s.TicketSection == filter.TicketSection.Value);
            }

            if (filter.TicketPriority != null)
            {
                query = query.Where(s => s.TicketPriority == filter.TicketPriority.Value);
            }

            #endregion

            #region state

            switch (filter.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;

                case FilterTicketState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;

                case FilterTicketState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
            }
            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDate_ASC:
                    query = query.OrderBy(s => s.CreateDate);
                    break;

                case FilterTicketOrder.CreateDate_DES:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
            }

            #endregion

            #region paging

            await filter.SetPaging(query);

            #endregion

            return filter;
        }

        public async Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, long userId)
        {
            if (string.IsNullOrEmpty(ticket.Text))
            {
                return AddTicketResult.Error;
            }

            // add ticket
            var newTicket = new Ticket
            {
                OwnerId = userId,
                Title = ticket.Title,
                IsReadByOwner = true,
                TicketPriority = ticket.TicketPriority,
                TicketSection = ticket.TicketSection,
                TicketState = TicketState.UnderProgress
            };

            await _contactRepository.AddTicket(newTicket);
            await _contactRepository.SaveChanges();

            // add ticket message
            var newMessage = new TicketMessage
            {
                TicketId = newTicket.Id,
                Text = ticket.Text,
                SenderId = userId,
            };

            await _contactRepository.AddTicketMessage(newMessage);
            await _contactRepository.SaveChanges();

            return AddTicketResult.Success;
        }

        public async Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId)
        {
            var ticket = await _contactRepository.GetTicketById(ticketId);

            if (ticket == null || ticket.OwnerId != userId)
            {
                return null;
            }

            return new TicketDetailDTO()
            {
                Ticket = ticket,
                TicketMessages = await _contactRepository.GetTicketMessageByTicketId(ticketId)
            };
        }

        #endregion

        #endregion
    }
}
