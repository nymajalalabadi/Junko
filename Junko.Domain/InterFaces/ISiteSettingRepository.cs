using Junko.Domain.Entities.Site;
using Junko.Domain.Entities.SiteSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface ISiteSettingRepository
    {
        Task<EmailSetting?> GetDefaultEmail();

        Task<SiteSetting?> GetDefaultSiteSetting();

        Task<List<Slider>> GetAllActiveSliders();
    }
}
