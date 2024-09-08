using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Orders;
using Junko.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.User.Controllers
{
    public class OrderController : UserBaseController
    {
        #region constructor

        private readonly IOrderService _orderService;

        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        #endregion

        #region add product to open order

        [AllowAnonymous]
        [HttpPost("add-product-to-order")]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderDTO order)
        {
            if (ModelState.IsValid)
            {

                if (User.Identity!.IsAuthenticated)
                {
                    if (await _orderService.CheckCountPrdocut(order.ProductId, order.Count))
                    {
                        await _orderService.AddProductToOpenOrder(User.GetUserId(), order);

                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "محصول مورد نظر با موفقیت ثبت شد", null);
                    }
                    else
                    {
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                         "تعداد محصولی که شما انتخاب کرده اید در فروشگاه موجود نیست", null);
                    }
                }
                else
                {
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                         " برای ثبت محصول ابتدا لاگین کنید", null);
                }

            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "در ثبت اطلاعات خطایی رخ داد", null);
        }

        #endregion


        #region open order

        [HttpGet("open-order")]
        public async Task<IActionResult> UserOpenOrder()
        {
            var openOrder = await _orderService.GetUserLatestOpenOrder(User.GetUserId());

            return View(openOrder);
        }

        #endregion

    }
}
