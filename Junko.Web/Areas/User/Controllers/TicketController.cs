using Junko.Application.Services.Interfaces;
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

        public IActionResult Index()
        {
            return View();
        }

        #endregion

    }
}
