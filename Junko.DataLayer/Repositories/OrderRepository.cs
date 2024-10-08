﻿using Junko.DataLayer.Context;
using Junko.Domain.Entities.ProductOrder;
using Junko.Domain.Entities.Products;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        #region constractor

        private readonly JunkoDbContext _context;

        public OrderRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region order

        public async Task<bool> GetAnyOrderByUserId(long userId)
        {
            return await _context.Orders.AnyAsync(o => o.UserId == userId && !o.IsPaid);
        }

        public async Task<Order?> GetUserLatestOpenOrder(long userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(d => d.ProductColor)
                .Include(o => o.OrderDetails).ThenInclude(d => d.ProductSize)
                .Include(o => o.OrderDetails).ThenInclude(d => d.Product).ThenInclude(d => d.ProductDiscounts)
                .Where(o => !o.IsDelete && !o.IsPaid)
                .FirstOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task<Order?> GetOrderById(long id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id); 
        }

        public async Task AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
        }
        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
        }

        #endregion

        #region order Details

        public async Task<OrderDetail?> GetOpenOrderDetail(long productId)
        {
            return await _context.OrderDetails.SingleOrDefaultAsync(d => d.ProductId == productId && !d.IsDelete);
        }

        public async Task<OrderDetail?> GetOpenOrderDetail(long productId, long ProductSizeId)
        {
            return await _context.OrderDetails.SingleOrDefaultAsync(d => d.ProductId == productId &&
            d.ProductSizeId == ProductSizeId && !d.IsDelete);
        }

        public async Task<OrderDetail?> GetOpenOrderDetail(long productId, long ProductSizeId ,long productColorId)
        {
            return await _context.OrderDetails.SingleOrDefaultAsync(d => d.ProductId == productId && 
            d.ProductSizeId == ProductSizeId && d.ProductColorId == productColorId && !d.IsDelete);
        }

        public async Task<OrderDetail?> GetOrderDetailById(long id)
        {
            return await _context.OrderDetails
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddOrderDetails(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
        }

        public void UpdateOrderDetails(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }

        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            orderDetail.IsDelete = true;
            _context.OrderDetails.Update(orderDetail);
        }

        #endregion

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
