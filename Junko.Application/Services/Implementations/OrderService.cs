using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.ProductOrder;
using Junko.Domain.Entities.Wallet;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Implementations
{
    public class OrderService : IOrderService
    {
        #region consractor

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWalletRepository _walletRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository,
            IWalletRepository walletRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _walletRepository = walletRepository;
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

            if (product!.ProductSizes.Any())
            {
                if (product.ProductSizes.Count >= count)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        public async Task<int> GetTotalOrderPriceForPayment(long userId)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);

            int totalPrice = 0;

            foreach (var detail in userOpenOrder!.OrderDetails)
            {
                var oneProductPrice = detail.ProductColor != null ? detail.Product.Price + detail.ProductColor.Price 
                    : detail.Product.Price;

                totalPrice += detail.Count * oneProductPrice ?? detail.Count * detail.Product.Price;
            }

            return totalPrice;
        }

        public async Task PayOrderProductPriceToSeller(long userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            foreach (var detail in openOrder!.OrderDetails)
            {
                var productPrice = detail.Product.Price;
                var productColorPrice = detail.ProductColor?.Price ?? 0;
                var productSize = detail.ProductSize?.Size ?? "بی سایز";
                var productCountSize = detail.ProductSize != null ? detail.Count : 0;
                var discount = 0;
                var totalPrice = detail.Count * (productPrice + productColorPrice) - discount;

                var sellerWallet = new SellerWallet()
                {
                    SellerId = detail.Product.SellerId,
                    Price = (int)Math.Ceiling(totalPrice * detail.Product.SiteProfit / (double)100),
                    TransactionType = TransactionType.Deposit,
                    Description = $"پرداخت مبلغ {totalPrice} تومان جهت فروش {detail.Product.Title} به تعداد {detail.Count} عدد با سهم تهیین شده ی {100 - detail.Product.SiteProfit} درصد"
                };

                await _walletRepository.AddWallet(sellerWallet);
                await _walletRepository.SaveChanges();

                detail.ProductPrice = totalPrice;
                detail.ProductColorPrice = productColorPrice;
                detail.Size = productSize;
                detail.ProductSize!.Count -= productCountSize;

                _orderRepository.UpdateOrderDetails(detail);
                await _orderRepository.SaveChanges();
            }

            openOrder.IsPaid = true;
            // todo: set description and tracing code in order

            _orderRepository.UpdateOrder(openOrder);
            await _orderRepository.SaveChanges();
        }

        #endregion

        #region order Details

        public async Task AddProductToOpenOrder(long userId, AddProductToOrderDTO order)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            if (order.ProductSizeId == null && order.ProductColorId == null)
            {
                var openOrderDetails = await _orderRepository.GetOpenOrderDetail(order.ProductId);

                if (openOrderDetails == null)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = openOrder!.Id,
                        ProductId = order.ProductId,
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

            if (order.ProductSizeId != null && order.ProductColorId == null)
            {
                var openOrderDetails = await _orderRepository.GetOpenOrderDetail(order.ProductId, order.ProductSizeId ?? 0);

                if (openOrderDetails == null)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = openOrder!.Id,
                        ProductId = order.ProductId,
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

            if (order.ProductSizeId != null && order.ProductColorId != null)
            {
                var openOrderDetails = await _orderRepository.GetOpenOrderDetail(order.ProductId, order.ProductSizeId ?? 0, order.ProductColorId ?? 0);

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
        }

        public async Task<UserOpenOrderDTO> GetUserOpenOrderDetail(long userId)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);

            return new UserOpenOrderDTO
            {
                UserId = userOpenOrder.UserId,
                Description = userOpenOrder.Description,
                Details = userOpenOrder.OrderDetails
                    .Where(s => !s.IsDelete)
                    .Select(s => new UserOpenOrderDetailItemDTO
                    {
                        DetailId = s.Id,
                        ProductId = s.ProductId,
                        Count = s.Count,
                        ProductPrice = s.Product.Price,
                        ProductTitle = s.Product.Title,
                        ProductImageName = s.Product.ImageName,
                        ProductColorId = s.ProductColorId,
                        ProductSizeId = s.ProductSizeId,
                        ProductColorPrice = s.ProductColor?.Price ?? 0,
                        ColorName = s.ProductColor?.ColorName ?? "بی رنگ",
                        size = s.ProductSize?.Size ?? "بی سایز",
                        DiscountPercentage = s.Product.ProductDiscounts
                        .OrderByDescending(a => a.CreateDate)
                        .FirstOrDefault(a => a.ExpireDate > DateTime.Now)?.Percentage
                    }).ToList()
            };
        }

        public async Task<bool> RemoveOrderDetail(long detailId, long userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            var orderDetail = openOrder!.OrderDetails.SingleOrDefault(s => s.Id == detailId);

            if (orderDetail == null)
            {
                return false;
            }

            _orderRepository.DeleteOrderDetail(orderDetail);
            await _orderRepository.SaveChanges();

            return true;
        }

        #endregion

        #endregion
    }
}
