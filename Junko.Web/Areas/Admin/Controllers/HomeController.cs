using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region index

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }

}
