using AndroidBitmapDescriptor = Huawei.Hms.Maps.Model.BitmapDescriptor;

namespace Xamarin.Forms.GoogleMaps.Android.Factories
{
    public interface IHMSBitmapDescriptorFactory
    {
        AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor);
    }
}