using Huawei.Hms.Maps;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System;

namespace Xamarin.Forms.GoogleMaps.Android.Extensions
{
    public static class MapViewExtensions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static Task<HuaweiMap> GetGoogleMapAsync(this MapView self)
        {
            var comp = new TaskCompletionSource<HuaweiMap>();
            self.GetMapAsync(new OnMapReadyCallback(map =>
            {
                comp.SetResult(map);
            }));

            return comp.Task;
        }
   }

    class OnMapReadyCallback : Java.Lang.Object, IOnMapReadyCallback
    {
        readonly Action<HuaweiMap> handler;

        public OnMapReadyCallback(Action<HuaweiMap> handler)
        {
            this.handler = handler;
        }

        void IOnMapReadyCallback.OnMapReady(HuaweiMap googleMap)
        {
            handler?.Invoke(googleMap);
        }
    }
}

