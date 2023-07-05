using System;
using Android.Graphics;
using AndroidBitmapDescriptor = Huawei.Hms.Maps.Model.BitmapDescriptor;
using AndroidBitmapDescriptorFactory = Huawei.Hms.Maps.Model.BitmapDescriptorFactory;

namespace Xamarin.Forms.GoogleMaps.Android.Factories
{
    public sealed class DefaultBitmapDescriptorFactory : IHMSBitmapDescriptorFactory
    {
        private static readonly Lazy<DefaultBitmapDescriptorFactory> _instance
            = new Lazy<DefaultBitmapDescriptorFactory>(() => new DefaultBitmapDescriptorFactory());

        public static DefaultBitmapDescriptorFactory Instance
        {
            get { return _instance.Value; }
        }
        
        private DefaultBitmapDescriptorFactory()
        {
        }
        
        public AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor)
        {
            switch (descriptor.Type)
            {
                case BitmapDescriptorType.Default:
                    return AndroidBitmapDescriptorFactory.DefaultMarker((float)((descriptor.Color.Hue * 360f) % 360f));
                case BitmapDescriptorType.Bundle:
					var context = FormsHuaweiMaps.Context;
                    using (var stream = context.Assets.Open(descriptor.BundleName))
                    {
                        return AndroidBitmapDescriptorFactory.FromBitmap(BitmapFactory.DecodeStream(stream));
                    }

                case BitmapDescriptorType.Stream:
                    if (descriptor.Stream.CanSeek && descriptor.Stream.Position > 0)
                    {
                        descriptor.Stream.Position = 0;
                    }
                    return AndroidBitmapDescriptorFactory.FromBitmap(BitmapFactory.DecodeStream(descriptor.Stream));
                case BitmapDescriptorType.AbsolutePath:
                    return AndroidBitmapDescriptorFactory.FromPath(descriptor.AbsolutePath);
                default:
                    return AndroidBitmapDescriptorFactory.DefaultMarker();
            }
        }
    }
}
