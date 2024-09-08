using Junko.Domain.Entities.ProductOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface IOrderRepository
    {
        #region Methods

        #region order

        Task<bool> GetAnyOrderByUserId(long userId);

        Task<Order?> GetUserLatestOpenOrder(long userId);

        Task<Order?> GetOrderById(long id);

        Task AddOrder(Order order);

        #endregion

        #region order Details

        Task<OrderDetail?> GetOpenOrderDetail(long productId);

        Task<OrderDetail?> GetOpenOrderDetail(long productId, long ProductSizeId);

        Task<OrderDetail?> GetOpenOrderDetail(long productId, long ProductSizeId, long productColorId);

        Task<OrderDetail?> GetOrderDetailById(long id);

        Task AddOrderDetails(OrderDetail orderDetail);

        void UpdateOrderDetails(OrderDetail orderDetail);

        #endregion

        Task SaveChanges();

        #endregion
    }
}
