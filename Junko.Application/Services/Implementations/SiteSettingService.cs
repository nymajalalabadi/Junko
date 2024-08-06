using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.SiteSetting;
using Junko.Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Implementations
{
    public class SiteSettingService : ISiteSettingService
    {
        #region constructor

        private readonly ISiteSettingRepository _siteSettingRepository;

        public SiteSettingService(ISiteSettingRepository siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        #endregion

        #region site settings

        public async Task<SiteSetting?> GetDefaultSiteSetting()
        {
            return await _siteSettingRepository.GetDefaultSiteSetting();
        }

        #endregion

    }

}
