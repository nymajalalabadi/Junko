using Junko.Domain.Entities.ProductOrder;
using Junko.Domain.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Interfaces
{
    public interface IOrderService
    {
        #region Methods

        #region order

        Task<long> AddOrderForUser(long userId);

        Task<Order?> GetUserLatestOpenOrder(long userId);

        Task<bool> CheckCountPrdocut(long ProductId, int count);

        Task<int> GetTotalOrderPriceForPayment(long userId);

        Task PayOrderProductPriceToSeller(long userId);

        #endregion

        #region order Details

        Task AddProductToOpenOrder(long userId, AddProductToOrderDTO order);

        Task<UserOpenOrderDTO> GetUserOpenOrderDetail(long userId);

        Task<bool> RemoveOrderDetail(long detailId, long userId);

        Task<bool> ChangeOrderDetailCount(long detailId, long userId, int count);

        #endregion

        #endregion
    }
}
