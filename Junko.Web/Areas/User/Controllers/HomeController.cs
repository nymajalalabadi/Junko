using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        #region constructor

        public HomeController()
        {
            
        }

        #endregion

        #region user dashboard

        [HttpGet("")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        #endregion
    }

}
