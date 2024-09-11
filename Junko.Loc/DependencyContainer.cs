using Junko.Application.Services.Implementations;
using Junko.Application.Services.Interfaces;
using Junko.DataLayer.Repositories;
using Junko.Domain.InterFaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Loc
{
    public class DependencyContainer
    {
        public static void RejosterService(IServiceCollection services)
        {
            #region service

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IContactService, ContactService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IViewRenderService, ViewRenderService>();

            services.AddScoped<ISiteSettingService, SiteSettingService>();

            services.AddScoped<ISellerService, SellerService>();

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IWalletService, WalletService>();

            services.AddScoped<IDiscountService, DiscountService>();

            #endregion

            #region repository

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IContactRepository, ContactRepository>();

            services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();

            services.AddScoped<ISellerRepository, SellerRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IWalletRepository, WalletRepository>();

            services.AddScoped<IDiscountRepository, DiscountRepository>();
            
            #endregion
        }
    }
}
