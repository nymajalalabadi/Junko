using Junko.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface IContactRepository
    {
        #region Methods

        Task AddContactUs(ContactUs contactUs);

        Task SaveChanges();

        #endregion
    }
}
