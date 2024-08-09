using Junko.DataLayer.Context;
using Junko.Domain.Entities.Site;
using Junko.Domain.Entities.SiteSetting;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class SiteSettingRepository : ISiteSettingRepository
    {
        #region constractor

        private readonly JunkoDbContext _context;

        public SiteSettingRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        public EmailSetting GetDefaultEmail()
        {
            var email = _context.EmailSettings.FirstOrDefault(s => !s.IsDelete && s.IsDefault);

            return email;
        }

        public async Task<SiteSetting?> GetDefaultSiteSetting()
        {
            return await _context.SiteSettings.SingleOrDefaultAsync(s => s.IsDefault && !s.IsDelete);
        }

        public async Task<List<Slider>> GetAllActiveSliders()
        {
            return await _context.Sliders.AsQueryable()
               .Where(s => s.IsActive && !s.IsDelete).ToListAsync();
        }

        #region site banners

        public async Task<List<SiteBanner>> GetSiteBannersByPlacement(List<BannerPlacement> placements)
        {
            return await _context.SiteBanners
                .AsQueryable()
                .Where(s => placements.Contains(s.BannerPlacement)).ToListAsync();
        }

        #endregion
    }
}
