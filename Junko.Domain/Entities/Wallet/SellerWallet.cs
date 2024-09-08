﻿using Junko.Domain.Entities.Common;
using Junko.Domain.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.Entities.Wallet
{
    public class SellerWallet : BaseEntity
    {
        #region properties

        public long SellerId { get; set; }

        public int Price { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? Description { get; set; }

        public TransactionType TransactionType { get; set; }

        #endregion

        #region relations

        public Seller Seller { get; set; }

        #endregion
    }

    public enum TransactionType
    {
        [Display(Name = "واریز")]
        Deposit,
        [Display(Name = "برداشت")]
        Withdrawal
    }

}
