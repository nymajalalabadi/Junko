using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Statics
{
    public static class SiteTools
    {
        #region default images

        public static string DefaultAvatar = "/img/defaults/avatar.jpg";

        #endregion 

        #region slider

        public static string SliderOrigin { get; set; } = "/img/slider/";

        #endregion

        #region banner

        public static string BannerOrigin { get; set; } = "/img/bg/";

        #endregion

        #region user avatar

        public static string UserAvatarOrigin { get; set; } = "/Content/Images/UserAvatar/origin/";

        public static string UserAvatarThumb { get; set; } = "/Content/Images/UserAvatar/Thumb/";

        #endregion

        #region products

        public static string ProductImage { get; set; } = "/content/images/product/origin/";


        public static string ProductThumbImage { get; set; } = "/content/images/product/thumb/";

        #endregion

        #region products Gallery

        public static string ProductGalleryImage { get; set; } = "/content/images/product-gallery/origin/";


        public static string ProductGalleryThumbImage { get; set; } = "/content/images/product-gallery/thumb/";

        #endregion

        #region uploader

        public static string UploadImage { get; set; } = "/img/upload/";

        #endregion
    }

}
