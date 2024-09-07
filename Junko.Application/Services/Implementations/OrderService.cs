using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.ProductOrder;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Implementations
{
    public class OrderService: IOrderService
    {
        #region consractor

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        #region order

        public async Task<long> AddOrderForUser(long userId)
        {
            var order = new Order()
            {
                UserId = userId
            };

            await _orderRepository.AddOrder(order);
            await _orderRepository.SaveChanges();

            return order.Id;
        }

        public async Task<Order?> GetUserLatestOpenOrder(long userId)
        {
            if (!await _orderRepository.GetAnyOrderByUserId(userId))
            {
                var order = new Order()
                {
                    UserId = userId
                };

                await _orderRepository.AddOrder(order);
                await _orderRepository.SaveChanges();

                return order;
            }

            var userOpenOrder = await _orderRepository.GetUserLatestOpenOrder(userId);

            return userOpenOrder;
        }

        public async Task<bool> CheckCountPrdocut(long ProductId, int count)
        {
            var product = await _productRepository.GetProductForOrder(ProductId);

            if (product!.ProductSizes.Count >= count)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region order Details

        public async Task AddProductToOpenOrder(long userId, AddProductToOrderDTO order)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            var openOrderDetails = await _orderRepository.GetOpenOrderDetail(order.ProductId, order.ProductColorId, order.ProductSizeId);

            if (openOrderDetails == null)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = openOrder!.Id,
                    ProductId = order.ProductId,
                    ProductColorId = order.ProductColorId,
                    ProductSizeId = order.ProductSizeId,
                    Count = order.Count
                };

                await _orderRepository.AddOrderDetails(orderDetail);
                await _orderRepository.SaveChanges();
            }
            else
            {
                openOrderDetails.Count += order.Count;
                
                _orderRepository.UpdateOrderDetails(openOrderDetails);
                await _orderRepository.SaveChanges();
            }
        }

        #endregion

        #endregion
    }
}
