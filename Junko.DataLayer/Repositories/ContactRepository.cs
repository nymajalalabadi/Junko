using Junko.DataLayer.Context;
using Junko.Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class ContactRepository : IContactRepository
    {
        #region constractor

        private readonly JunkoDbContext _context;

        public ContactRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods



        #endregion
    }
}
