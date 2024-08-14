using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}
