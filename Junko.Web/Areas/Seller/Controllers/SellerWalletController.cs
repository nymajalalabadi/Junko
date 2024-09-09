using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Wallet;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Seller.Controllers
{
    public class SellerWalletController : SellerBaseController
    {
        #region constructor

        private readonly IWalletService _walletService;
        private readonly ISellerService _sellerService;

        public SellerWalletController(IWalletService walletService, ISellerService sellerService)
        {
            _walletService = walletService;
            _sellerService = sellerService;
        }

        #endregion

        #region index

        [HttpGet("seller-wallet")]
        public async Task<IActionResult> Index(FilterSellerWalletDTO filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            if (seller == null)
            {
                return NotFound();
            }

            filter.TakeEntity = 5;
            filter.SellerId = seller.Id;

            var result = await _walletService.FilterSellerWallet(filter);

            return View(result);
        }

        #endregion
    }

}
