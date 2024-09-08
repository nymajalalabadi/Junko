﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Products
{
    public class CreateProductColorDTO
    {
        [Display(Name = "رنگ")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ColorName { get; set; }

        [Display(Name = "کد رنگ")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ColorCode { get; set; }

        public int? Price { get; set; }
    }

}
