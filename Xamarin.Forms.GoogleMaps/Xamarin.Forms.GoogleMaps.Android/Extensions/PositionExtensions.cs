﻿using System;
using System.Collections.Generic;
using Huawei.Hms.Maps.Model;
using System.Linq;
namespace Xamarin.Forms.GoogleMaps.Android
{
    public static class PositionExtensions
    {
        public static LatLng ToLatLng(this Position self)
        {
            return new LatLng(self.Latitude, self.Longitude);
        }

        public static IList<LatLng> ToLatLngs(this IList<Position> self)
        {
            return self.Select(ToLatLng).ToList();
        }
    }
}

