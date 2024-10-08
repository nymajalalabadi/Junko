﻿using Junko.Domain.Entities.Common;
using Junko.Domain.Entities.Contacts;
using Junko.Domain.Entities.Discount;
using Junko.Domain.Entities.ProductOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.Entities.Account
{
    public class User : BaseEntity
    {
        #region properties

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }

        [Display(Name = "کلمه ی عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string EmailActiveCode { get; set; }

        [Display(Name = "ایمیل فعال / غیرفعال")]
        public bool IsEmailActive { get; set; }

        [Display(Name = "تلفن همراه")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string MobileActiveCode { get; set; }

        [Display(Name = "موبایل فعال / غیرفعال")]
        public bool IsMobileActive { get; set; }

        [Display(Name = "تصویر آواتار")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Avatar { get; set; }

        [Display(Name = "بلاک شده / نشده")]
        public bool IsBlocked { get; set; }

        public bool IsAdmin { get; set; }

        #endregion

        #region relations

        public ICollection<ContactUs> ContactUses { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<TicketMessage> TicketMessages { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<ProductDiscountUse> productDiscountUses { get; set; }

        #endregion
    }
}
