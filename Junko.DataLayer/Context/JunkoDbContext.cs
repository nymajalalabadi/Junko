using Azure;
using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Contacts;
using Junko.Domain.Entities.ProductOrder;
using Junko.Domain.Entities.Products;
using Junko.Domain.Entities.Site;
using Junko.Domain.Entities.SiteSetting;
using Junko.Domain.Entities.Store;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Context
{
    public class JunkoDbContext : DbContext
    {
        public JunkoDbContext(DbContextOptions<JunkoDbContext> options) : base(options) { }

        #region Db Set

        public DbSet<User> Users { get; set; }

        public DbSet<EmailSetting> EmailSettings { get; set; }

        public DbSet<SiteSetting> SiteSettings { get; set; }

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<SiteBanner> SiteBanners { get; set; }

        public DbSet<ContactUs> ContactUs { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketMessage> TicketMessages { get; set; }

        public DbSet<Seller> Sellers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductColor> ProductColors { get; set; }

        public DbSet<ProductSize> ProductSizes { get; set; }

        public DbSet<ProductGallery> ProductGalleries { get; set; }

        public DbSet<ProductFeature> ProductFeatures { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }

            #region Seed Data

            var date = DateTime.MinValue;

            modelBuilder.Entity<EmailSetting>().HasData(new EmailSetting()
            {
                CreateDate = date,
                DisplayName = "Junko",
                EnableSSL = true,
                From = "nymasteam@gmail.com",
                Id = 1,
                IsDefault = true,
                IsDelete = false,
                Password = "fuqijttnofjradmh",
                Port = 587,
                SMTP = "smtp.gmail.com"
            });

            modelBuilder.Entity<SiteSetting>().HasData(new SiteSetting()
            {
                CreateDate = date,
                Mobile = "09333333347",
                Address = "canada",
                Email = "nymasteam@gmail.com",
                Id = 1,
                IsDefault = true,
                IsDelete = false,
                Phone = "0442337358",
                CopyRight = "nima jalalabadi",
                FooterText = "nymasteam@gmail.com",
                LastUpdateDate = date,
                AboutUs = " هیچی",
                MapScript = "null"
            });

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
