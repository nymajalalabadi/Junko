using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Products
{
    public class EditProductGalleryDTO
    {
        [Display(Name = "اولویت نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int DisplayPriority { get; set; }

        public string ImageName { get; set; }

        [Display(Name = "تصویر")]
        public IFormFile? Image { get; set; }
    }

    public enum EditProductGalleryResult
    {
        Success,
        NotForUserProduct,
        ProductNotFound
    }

}
