using Junko.Domain.Entities.Common;
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

        [Display(Name = "Email")]
        [MaxLength(200, ErrorMessage = "{0} Cannot Be Longer Than {1} Characters")]
        [EmailAddress(ErrorMessage = "Your Email Is Invalid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} Is Required")]
        [MaxLength(200, ErrorMessage = "{0} Cannot Be Longer Than {1} Characters")]
        public string EmailActiveCode { get; set; }

        [Display(Name = "Enabled Email / Disabled Email")]
        public bool IsEmailActive { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "The {0} Is Required")]
        [MaxLength(200, ErrorMessage = "{0} Cannot Be Longer Than {1} Characters")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "The {0} Is Required")]
        [MaxLength(20, ErrorMessage = "{0} Cannot Be Longer Than {1} Characters")]
        public string MobileActiveCode { get; set; }

        [Display(Name = "Enabled Mobile / Disabled Mobile")]
        public bool IsMobileActive { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "The {0} Is Required")]
        [MaxLength(200, ErrorMessage = "{0} Cannot Be Longer Than {1} Characters")]
        public string Password { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The {0} Is Required")]
        [MaxLength(200, ErrorMessage = "{0} Cannot Be Longer Than {1} Characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The {0} Is Required")]
        [MaxLength(200, ErrorMessage = "{0} Cannot Be Longer Than {1} Characters")]
        public string LastName { get; set; }

        [Display(Name = "Avatar Picture")]
        [MaxLength(200, ErrorMessage = "{0} Cannot Be Longer Than {1} Characters")]
        public string? Avatar { get; set; }

        [Display(Name = "Blocked / Unblocked")]
        public bool IsBlocked { get; set; }

        #endregion

        #region relations

        #endregion
    }
}
