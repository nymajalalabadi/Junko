using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.Contacts;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.ContactUs;

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

        public async Task CreateContactUs(CreateContactUsDTO contact, string userIp, long? userId)
        {
            var newContact = new ContactUs
            {
                UserId = userId != null && userId.Value != 0 ? userId.Value : (long?)null,
                Subject = contact.Subject,
                Email = contact.Email,
                UserIp = userIp,
                Text = contact.Text,
                FullName = contact.FullName
            };

            await _contactRepository.AddContactUs(newContact);
            await _contactRepository.SaveChanges();
        }

        #endregion
    }
}
