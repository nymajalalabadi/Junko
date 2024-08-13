﻿using Junko.Domain.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Interfaces
{
    public interface ISellerService
    {
        #region Methods

        Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter);

        Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long userId);

        Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId);

        Task<EditRequestSellerResult> EditRequestSeller(EditRequestSellerDTO request, long currentUserId);

        Task<bool> AcceptSellerRequest(long requestId);

        #endregion
    }
}
