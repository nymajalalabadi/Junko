﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Orders
{
    public class UserOpenOrderDTO
    {
        public long UserId { get; set; }

        public string Description { get; set; }

        public List<UserOpenOrderDetailItemDTO> Details { get; set; }

        public int GetTotalPrice()
        {
            return Details.Sum(s => s.Count * (s.ProductPrice + s.ProductColorPrice));
        }

        public int GetTotalDiscounts()
        {
            return Details.Sum(s => s.GetOrderDetailWithDiscountPriceAmount());
        }
    }

}
