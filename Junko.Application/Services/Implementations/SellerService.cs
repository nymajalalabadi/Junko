using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.Store;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Store;
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

        #endregion
    }
}
