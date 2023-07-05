﻿using System;
using Huawei.Hms.Maps.Model;
using Xamarin.Forms.GoogleMaps.Android.Extensions;

namespace Xamarin.Forms.GoogleMaps.Android
{
    public static class VisibleRegionExtensions
    {
        public static MapRegion ToRegion(this VisibleRegion visibleRegion)
        {
            return new MapRegion(
                visibleRegion.NearLeft.ToPosition(),
                visibleRegion.NearRight.ToPosition(),
                visibleRegion.FarLeft.ToPosition(),
                visibleRegion.FarRight.ToPosition()
            );
        }        
    }
}
