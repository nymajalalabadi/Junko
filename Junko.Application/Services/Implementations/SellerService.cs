using Azure.Core;
using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.Store;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Store;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Implementations
{
    public class SellerService : ISellerService
    {
        #region consractor

        private readonly ISellerRepository _sellerRepository;
        private readonly IUserRepository _userRepository;

        public SellerService(ISellerRepository sellerRepository, IUserRepository userRepository)
        {
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region Methods

        public async Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter)
        {
            var query = await _sellerRepository.GetSellerQuery();

            #region filter

            if (filter.UserId != 0 && filter.UserId != null)
            {
                query = query.Where(s => s.UserId.Equals(filter.UserId));
            }

            if (!string.IsNullOrEmpty(filter.StoreName))
            {
                query = query.Where(s => EF.Functions.Like(s.StoreName, $"%{filter.StoreName}%"));
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(s => EF.Functions.Like(s.Email, $"%{filter.Email}%"));
            }

            if (!string.IsNullOrEmpty(filter.Mobile))
            {
                query = query.Where(s => EF.Functions.Like(s.Mobile, $"%{filter.Mobile}%"));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                query = query.Where(s => EF.Functions.Like(s.Address, $"%{filter.Address}%"));
            }

            #endregion

            #region state

            switch (filter.State)
            {
                case FilterSellerState.All:
                    query = query.Where(s => !s.IsDelete);
                    break;

                case FilterSellerState.Accepted:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Accepted && !s.IsDelete);
                    break;

                case FilterSellerState.UnderProgress:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress && !s.IsDelete);
                    break;

                case FilterSellerState.Rejected:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Rejected && !s.IsDelete);
                    break;
            }

            #endregion

            #region Paging

            await filter.SetPaging(query);

            #endregion

            return filter;
        }

        public async Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return RequestSellerResult.HasNotPermission;
            }

            if (user.IsBlocked)
            {
                return RequestSellerResult.HasNotPermission;
            }

            var hasUnderProgressRequest = await _sellerRepository.HasUnderProgressRequest(userId);

            if (hasUnderProgressRequest)
            {
                return RequestSellerResult.HasUnderProgressRequest;
            }

            var newSeller = new Seller
            {
                UserId = userId,
                StoreName = seller.StoreName,
                Address = seller.Address,
                Email = seller.Email,
                StoreAcceptanceState = StoreAcceptanceState.UnderProgress
            };

            await _sellerRepository.AddSeller(newSeller);
            await _sellerRepository.SaveChanges();

            return RequestSellerResult.Success;
        }

        public async Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId)
        {
            var seller = await _sellerRepository.GetSellerById(id);

            if (seller == null || seller.UserId != currentUserId)
            {
                return null;
            }

            return new EditRequestSellerDTO
            {
                Id = seller.Id,
                Email = seller.Email,
                Address = seller.Address,
                StoreName = seller.StoreName
            };

        }

        public async Task<EditRequestSellerResult> EditRequestSeller(EditRequestSellerDTO request, long currentUserId)
        {
            var seller = await _sellerRepository.GetSellerById(request.Id);

            if (seller == null || seller.UserId != currentUserId)
            {
                return EditRequestSellerResult.NotFound;
            }

            seller.Email = request.Email;
            seller.Mobile = request.Phone;
            seller.Address = request.Address;
            seller.StoreName = request.StoreName;
            seller.StoreAcceptanceState = StoreAcceptanceState.UnderProgress;

            _sellerRepository.UpdateSeller(seller);
            await _sellerRepository.SaveChanges();

            return EditRequestSellerResult.Success;
        }

        public async Task<bool> AcceptSellerRequest(long requestId)
        {
            var sellerRequest = await _sellerRepository.GetSellerById(requestId);

            if (sellerRequest == null)
            {
                return false;
            }

            sellerRequest.StoreAcceptanceState = StoreAcceptanceState.Accepted;
            sellerRequest.StoreAcceptanceDescription = "اطلاعات پنل فروشندگی شما تایید شده است";

            _sellerRepository.UpdateSeller(sellerRequest);
            await _sellerRepository.SaveChanges();

            return true;
        }

        public async Task<bool> RejectSellerRequest(RejectItemDTO reject)
        {
            var sellerRequest = await _sellerRepository.GetSellerById(reject.Id);

            if (sellerRequest == null)
            {
                return false;
            }

            sellerRequest.StoreAcceptanceState = StoreAcceptanceState.Rejected;
            sellerRequest.StoreAcceptanceDescription = reject.RejectMessage;

            _sellerRepository.UpdateSeller(sellerRequest);
            await _sellerRepository.SaveChanges();

            return true;
        }

        #endregion
    }
}
