using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Products
{
    public class EditProductDTO
    {
        public long Id { get; set; }

        [Display(Name = "نام محصول")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Title { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        public string? Description { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        public string ImageName { get; set; }

        [Display(Name = "عکس اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile? AvatarImage { get; set; }

        public List<CreateProductColorDTO> ProductColors { get; set; }

        public List<CreateProductSizeDTO> ProductSizes { get; set; }

        public List<long> SelectedCategories { get; set; }

    }
    public enum EditProductResult
    {
        NotFound,
        NotForUser,
        Success
    }

}
