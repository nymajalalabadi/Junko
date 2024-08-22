using Junko.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Junko.Application.Extensions;

namespace Junko.Web.Http
{
    public class CheckSellerStateAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private ISellerService _sellerService;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _sellerService = (ISellerService)context.HttpContext.RequestServices.GetService(typeof(ISellerService));

            if (context.HttpContext.User.Identity!.IsAuthenticated)
            {
                var userId = context.HttpContext.User.GetUserId();

                if (!_sellerService.HasUserAnyActiveSellerPanel(userId).Result)
                {
                    context.Result = new RedirectResult("/user");
                }
            }
        }
    }
}
