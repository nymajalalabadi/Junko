﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Products
{
    public class CreateProductSizeDTO
    {
        [Display(Name = "اندازه")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Size { get; set; }

        [Display(Name = "تعداد محصول")]
        public int? Count { get; set; }
    }
}
