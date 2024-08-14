using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface IProductRepository
    {
        #region Methods

        Task<IQueryable<Product>> GetProductQuery();


        #endregion
    }
}
