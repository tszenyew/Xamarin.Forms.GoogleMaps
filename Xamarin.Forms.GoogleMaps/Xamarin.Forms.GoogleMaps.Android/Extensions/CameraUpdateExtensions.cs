﻿using System;
using GCameraUpdateFactory = Huawei.Hms.Maps.CameraUpdateFactory;
using GCameraUpdate = Huawei.Hms.Maps.CameraUpdate;
using Xamarin.Forms.GoogleMaps.Internals;

namespace Xamarin.Forms.GoogleMaps.Android.Extensions
{
    public static class CameraUpdateExtensions
    {
        public static GCameraUpdate ToAndroid(this CameraUpdate self, float scaledDensity)
        {
            switch (self.UpdateType)
            {
                case CameraUpdateType.LatLng:
                    return GCameraUpdateFactory.NewLatLng(self.Position.ToLatLng());
                case CameraUpdateType.LatLngZoom:
                    return GCameraUpdateFactory.NewLatLngZoom(self.Position.ToLatLng(), (float) self.Zoom);
                case CameraUpdateType.LatLngBounds:
                    return GCameraUpdateFactory.NewLatLngBounds(self.Bounds.ToLatLngBounds(), (int)(self.Padding * scaledDensity)); // TODO: convert from px to pt. Is this collect? (looks like same iOS Maps)
                case CameraUpdateType.CameraPosition:
                    return GCameraUpdateFactory.NewCameraPosition(self.CameraPosition.ToAndroid());
                default:
                    throw new ArgumentException($"{nameof(self)} UpdateType is not supported.");
            }
        }
    }
}
