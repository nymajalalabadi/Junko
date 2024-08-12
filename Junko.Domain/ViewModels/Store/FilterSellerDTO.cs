using Junko.Domain.Entities.Store;
using Junko.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Store
{
    public class FilterSellerDTO : Paging<Seller>
    {
        public long? UserId { get; set; }

        public string? StoreName { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public string? Address { get; set; }

        public FilterSellerState State { get; set; }
    }

    public enum FilterSellerState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }

}
