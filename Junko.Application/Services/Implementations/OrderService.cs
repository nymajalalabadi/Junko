using Junko.Application.Services.Interfaces;
using Junko.Domain.InterFaces;
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

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion

        #region Methods



        #endregion
    }
}
