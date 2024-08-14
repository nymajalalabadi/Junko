using Junko.Domain.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Interfaces
{
    public interface IProductService
    {
        #region Methods

        Task<FilterProductDTO> FilterProducts(FilterProductDTO filter);

        #endregion
    }
}
