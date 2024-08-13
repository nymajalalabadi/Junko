using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Store;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Admin.Controllers
{
    public class SellerController : AdminBaseController
    {
        #region constructor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region seller requests

        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            var model = await _sellerService.FilterSellers(filter);

            return View(model);
        }

        #endregion

    }
}
