using Junko.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.Entities.Products
{
    public class ProductSize : BaseEntity
    {
        #region properties

        [Display(Name = "اندازه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Size { get; set; }

        #endregion

        #region relations

        public ICollection<ProductSelectedColorSize> ProductSelectedColorSizes { get; set; }


        #endregion
    }
}
