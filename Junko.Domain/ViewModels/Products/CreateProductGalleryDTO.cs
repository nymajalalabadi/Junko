using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Products
{
    public class CreateProductGalleryDTO
    {
        [Display(Name = "اولویت نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int DisplayPriority { get; set; }

        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public IFormFile AvatarImage { get; set; }
    }

    public enum CreateProductGalleryResult
    {
        Success,
        NotForUserProduct,
        ImageIsNull,
        ProductNotFound
    }

}
