using System;
using Huawei.Hms.Maps.Model;
namespace Xamarin.Forms.GoogleMaps.Android.Extensions
{
    public static class LatLngExtensions
    {
        public static Position ToPosition(this LatLng self)
        {
            return new Position(self.Latitude, self.Longitude);
        }
    }
}

