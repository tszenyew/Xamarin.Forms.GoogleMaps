using System;
using Android.App;
using Android.Content;
using Huawei.Hms.Maps;
using Android.OS;
using Xamarin.Forms.GoogleMaps.Android;
using Xamarin.Forms.GoogleMaps.Android.Factories;
using Huawei.Hms.Api;

namespace Xamarin
{
    public static class FormsHuaweiMaps
    {
        public static bool IsInitialized { get; private set; }

        public static Context Context { get; private set; }

        public static void Init(Activity activity, Bundle bundle, string APIKEY, HMSPlatformConfig config = null)
        {
            if (IsInitialized)
                return;

            Context = activity;

            MapRenderer.Bundle = bundle;
            MapRenderer.Config = config ?? new HMSPlatformConfig();

#pragma warning disable 618
            if (HuaweiApiAvailability.Instance.IsHuaweiMobileServicesAvailable(Context) == ConnectionResult.Success)
#pragma warning restore 618
            {
                try
                {
                    MapsInitializer.SetApiKey(APIKEY);
                    MapsInitializer.Initialize(Context);
                    IsInitialized = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("HMS Services Not Found");
                    Console.WriteLine("Exception: {0}", e);
                }
            }

            GeocoderBackend.Register(Context);
        }
    }
}