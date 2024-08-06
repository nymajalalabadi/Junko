using Junko.DataLayer.Context;
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

        public async Task<EmailSetting?> GetDefaultEmail()
        {
            return await _context.EmailSettings.FirstOrDefaultAsync(s => !s.IsDelete && s.IsDefault);
        }

        public async Task<SiteSetting?> GetDefaultSiteSetting()
        {
            return await _context.SiteSettings.SingleOrDefaultAsync(s => s.IsDefault && !s.IsDelete);
        }
    }
}
