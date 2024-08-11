using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.ContactUs;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region constructor

        private readonly IContactService _contactService;

        public TicketController(IContactService contactService)
        {
            _contactService = contactService;
        }

        #endregion

        #region list

        [HttpGet("tickets")]
        public async Task<IActionResult> Index(FilterTicketDTO filter)
        {
            filter.UserId = User.GetUserId(); 

            filter.OrderBy = FilterTicketOrder.CreateDate_DES;
            filter.FilterTicketState = FilterTicketState.NotDeleted;

            var result = await _contactService.FilterTickets(filter);

            return View(result);
        }

        #endregion

        #region add tiket

        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactService.AddUserTicket(ticket, User.GetUserId());

                switch (result)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                        break;

                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "پاسخ شما به زودی ارسال خواهد شد";
                        return RedirectToAction("Index");
                }
            }

            return View(ticket);
        }

        #endregion

        #region show ticket detail

        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetail(long ticketId)
        {
            var ticket = await _contactService.GetTicketForShow(ticketId, User.GetUserId());

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        #endregion

        #region Close Ticket

        [HttpGet("CloseTicket/{ticketId}")]
        public async Task<IActionResult> CloseTicket(long ticketId)
        {
            var result = await _contactService.CloseTicket(ticketId);

            if (result)
            {
                TempData[SuccessMessage] = "تیکت شما با موفقیت بسته شد";
            }
            else
            {
                TempData[SuccessMessage] = "تیکت شما با مشکل رو به رو شد ";
            }

            return RedirectToAction("Index");
        }

        #endregion

    }
}
