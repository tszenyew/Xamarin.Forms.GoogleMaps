using Xamarin.Forms.GoogleMaps.Android.Factories;
using static Huawei.Hms.Maps.HuaweiMap;

namespace Xamarin.Forms.GoogleMaps.Android
{
    public sealed class HMSPlatformConfig
    {
        public IHMSBitmapDescriptorFactory BitmapDescriptorFactory { get; set; }
        public IInfoWindowAdapter MapInfoWindowAdaper { get; set; }
    }
}