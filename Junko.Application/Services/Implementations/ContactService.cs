using Junko.Application.Services.Interfaces;
using Junko.Domain.InterFaces;

namespace Junko.Application.Services.Implementations
{
    public class ContactService : IContactService
    {
        #region consractor

        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        #endregion

        #region Methods



        #endregion
    }
}
