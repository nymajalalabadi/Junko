using Junko.Domain.Entities.Site;
using Junko.Domain.Entities.SiteSetting;

namespace Junko.Application.Services.Interfaces
{
    public interface ISiteSettingService
    {
        #region site settings

        Task<SiteSetting?> GetDefaultSiteSetting();

        #endregion

        #region slider

        Task<List<Slider>> GetAllActiveSliders();

        #endregion

        #region site banners

        Task<List<SiteBanner>> GetSiteBannersByPlacement(List<BannerPlacement> placements);

        #endregion

    }

}
