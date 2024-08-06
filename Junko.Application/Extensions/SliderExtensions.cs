using Junko.Application.Statics;
using Junko.Domain.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Extensions
{
    public static class SliderExtensions
    {
        public static string GetSliderImageAddress(this Slider slider)
        {
            return SiteTools.SliderOrigin + slider.ImageName;
        }
    }

}