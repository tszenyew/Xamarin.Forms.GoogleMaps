using System;
using System.Collections.Generic;
using Huawei.Hms.Maps;
using NativeGroundOverlay = Huawei.Hms.Maps.Model.GroundOverlay;
using Huawei.Hms.Maps.Model;
using Xamarin.Forms.GoogleMaps.Android.Extensions;
using System.Linq;
using Xamarin.Forms.GoogleMaps.Android.Factories;
using Xamarin.Forms.GoogleMaps.Android;
using Android.Widget;
using Android.Content;

namespace Xamarin.Forms.GoogleMaps.Logics.Android
{
    public class GroundOverlayLogic : DefaultGroundOverlayLogic<NativeGroundOverlay, HuaweiMap>
    {
        protected override IList<GroundOverlay> GetItems(Map map) => map.GroundOverlays;

        private readonly Context _context;
        private readonly IHMSBitmapDescriptorFactory _bitmapDescriptorFactory;

        public GroundOverlayLogic(
            Context context, 
            IHMSBitmapDescriptorFactory bitmapDescriptorFactory)
        {
            _context = context;
            _bitmapDescriptorFactory = bitmapDescriptorFactory;
        }

        public override void Register(HuaweiMap oldNativeMap, Map oldMap, HuaweiMap newNativeMap, Map newMap)
        {
            base.Register(oldNativeMap, oldMap, newNativeMap, newMap);

            if (newNativeMap != null)
            {
                newNativeMap.GroundOverlayClick += OnGroundOverlayClick;
            }
        }

        public override void Unregister(HuaweiMap nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                nativeMap.GroundOverlayClick -= OnGroundOverlayClick;
            }

            base.Unregister(nativeMap, map);
        }

        protected override NativeGroundOverlay CreateNativeItem(GroundOverlay outerItem)
        {
            var factory = _bitmapDescriptorFactory ?? DefaultBitmapDescriptorFactory.Instance;
            var nativeDescriptor = factory.ToNative(outerItem.Icon);

            var opts = new GroundOverlayOptions()
                .PositionFromBounds(outerItem.Bounds.ToLatLngBounds())
                .Clickable(outerItem.IsClickable)
                .InvokeBearing(outerItem.Bearing)
                .InvokeImage(nativeDescriptor)
                .InvokeTransparency(outerItem.Transparency)
                .InvokeZIndex(outerItem.ZIndex);

            var overlay = NativeMap.AddGroundOverlay(opts);

            // If the pin has an IconView set this method will convert it into an icon for the marker
            if (outerItem?.Icon?.Type == BitmapDescriptorType.View)
            {
                overlay.Visible = false; // Will become visible once the iconview is ready.
                TransformXamarinViewToAndroidBitmap(outerItem, overlay);
            }
            else
            {
                overlay.Visible = outerItem.IsVisible;
            }

            // associate pin with marker for later lookup in event handlers
            outerItem.NativeObject = overlay;
            return overlay;
        }

        protected override NativeGroundOverlay DeleteNativeItem(GroundOverlay outerItem)
        {
            var nativeOverlay = outerItem.NativeObject as NativeGroundOverlay;
            if (nativeOverlay == null)
                return null;
            nativeOverlay.Remove();
            outerItem.NativeObject = null;

            return nativeOverlay;
        }

        void OnGroundOverlayClick(object sender, HuaweiMap.GroundOverlayClickEventArgs e)
        {
            // clicked ground overlay
            var nativeItem = e.P0;

            // lookup overlay
            var targetOuterItem = GetItems(Map).FirstOrDefault(
                outerItem => ((NativeGroundOverlay)outerItem.NativeObject).Id == nativeItem.Id);

            // only consider event handled if a handler is present.
            // Else allow default behavior of displaying an info window.
            targetOuterItem?.SendTap();
        }

        protected override void OnUpdateBearing(GroundOverlay outerItem, NativeGroundOverlay nativeItem)
        {
            nativeItem.Bearing = outerItem.Bearing;
        }

        protected override void OnUpdateBounds(GroundOverlay outerItem, NativeGroundOverlay nativeItem)
        {
            nativeItem.SetPositionFromBounds(outerItem.Bounds.ToLatLngBounds()); 
        }

        protected override void OnUpdateIcon(GroundOverlay outerItem, NativeGroundOverlay nativeItem)
        {
            if (outerItem.Icon != null && outerItem.Icon.Type == BitmapDescriptorType.View)
            {
                // If the pin has an IconView set this method will convert it into an icon for the marker
                TransformXamarinViewToAndroidBitmap(outerItem, nativeItem);
            }
            else
            {
                var factory = _bitmapDescriptorFactory ?? DefaultBitmapDescriptorFactory.Instance;
                var nativeDescriptor = factory.ToNative(outerItem.Icon);
                nativeItem.SetImage(nativeDescriptor);
            }
        }

        protected override void OnUpdateIsClickable(GroundOverlay outerItem, NativeGroundOverlay nativeItem)
        {
            nativeItem.Clickable = outerItem.IsClickable;
        }

        protected override void OnUpdateTransparency(GroundOverlay outerItem, NativeGroundOverlay nativeItem)
        {
            nativeItem.Transparency = outerItem.Transparency;
        }

        protected override void OnUpdateZIndex(GroundOverlay outerItem, NativeGroundOverlay nativeItem)
        {
            nativeItem.ZIndex = outerItem.ZIndex;
        }

        private async void TransformXamarinViewToAndroidBitmap(GroundOverlay outerItem, NativeGroundOverlay nativeItem)
        {
            if (outerItem?.Icon?.Type == BitmapDescriptorType.View && outerItem.Icon?.View != null)
            {
                var iconView = outerItem.Icon.View;
                var nativeView = await Utils.ConvertFormsToNative(
                    iconView,
                    new Rectangle(0, 0, (double)Utils.DpToPx((float)iconView.WidthRequest), (double)Utils.DpToPx((float)iconView.HeightRequest)),
                    Platform.Android.Platform.CreateRendererWithContext(iconView, _context));
                var otherView = new FrameLayout(nativeView.Context);
                nativeView.LayoutParameters = new FrameLayout.LayoutParams(Utils.DpToPx((float)iconView.WidthRequest), Utils.DpToPx((float)iconView.HeightRequest));
                otherView.AddView(nativeView);
                nativeItem.SetImage(await Utils.ConvertViewToBitmapDescriptor(otherView));
                //nativeItem.SetAnchor((float)iconView.AnchorX, (float)iconView.AnchorY);
                nativeItem.Visible = true;
            }
        }
    }
}


