using Huawei.Hms.Maps.Model;
using System.Collections.Generic;
using Huawei.Hms.Maps;
using System.Linq;
using System.ComponentModel;
using Xamarin.Forms.GoogleMaps.Android;
using Xamarin.Forms.GoogleMaps.Android.Extensions;
using Android.Widget;
using System;
using Android.Content;
using Xamarin.Forms.GoogleMaps.Android.Factories;

namespace Xamarin.Forms.GoogleMaps.Logics.Android
{
    public class PinLogic : DefaultPinLogic<Marker, HuaweiMap>
    {
        protected override IList<Pin> GetItems(Map map) => map.Pins;

        private volatile bool _onMarkerEvent = false;
        private Pin _draggingPin;
        private volatile bool _withoutUpdateNative = false;

        private readonly Context _context;
        private readonly IHMSBitmapDescriptorFactory _bitmapDescriptorFactory;
        private readonly Action<Pin, MarkerOptions> _onMarkerCreating;
        private readonly Action<Pin, Marker> _onMarkerCreated;
        private readonly Action<Pin, Marker> _onMarkerDeleting;
        private readonly Action<Pin, Marker> _onMarkerDeleted;

        public PinLogic(
            Context context,
            IHMSBitmapDescriptorFactory bitmapDescriptorFactory,
            Action<Pin, MarkerOptions> onMarkerCreating,
            Action<Pin, Marker> onMarkerCreated, 
            Action<Pin, Marker> onMarkerDeleting,
            Action<Pin, Marker> onMarkerDeleted)
        {
            _bitmapDescriptorFactory = bitmapDescriptorFactory;
            _context = context;
            _onMarkerCreating = onMarkerCreating;
            _onMarkerCreated = onMarkerCreated;
            _onMarkerDeleting = onMarkerDeleting;
            _onMarkerDeleted = onMarkerDeleted;
        }

        public override void Register(HuaweiMap oldNativeMap, Map oldMap, HuaweiMap newNativeMap, Map newMap)
        {
            base.Register(oldNativeMap, oldMap, newNativeMap, newMap);

            if (newNativeMap != null)
            {
                newNativeMap.InfoWindowClick += OnInfoWindowClick;
                newNativeMap.InfoWindowLongClick += OnInfoWindowLongClick;
                newNativeMap.MarkerClick += OnMakerClick;
                newNativeMap.InfoWindowClose += OnInfoWindowClose;
                newNativeMap.MarkerDragStart += OnMarkerDragStart;
                newNativeMap.MarkerDragEnd += OnMarkerDragEnd;
                newNativeMap.MarkerDrag += OnMarkerDrag;
            }
        }

        public override void Unregister(HuaweiMap nativeMap, Map map)
        {
            if (nativeMap != null)
            {
                nativeMap.MarkerDrag -= OnMarkerDrag;
                nativeMap.MarkerDragEnd -= OnMarkerDragEnd;
                nativeMap.MarkerDragStart -= OnMarkerDragStart;
                nativeMap.MarkerClick -= OnMakerClick;
                nativeMap.InfoWindowClose -= OnInfoWindowClose;
                nativeMap.InfoWindowClick -= OnInfoWindowClick;
                nativeMap.InfoWindowLongClick -= OnInfoWindowLongClick;
            }

            base.Unregister(nativeMap, map);
        }

        protected override Marker CreateNativeItem(Pin outerItem)
        {
            var opts = new MarkerOptions()
                .InvokePosition(new LatLng(outerItem.Position.Latitude, outerItem.Position.Longitude))
                .InvokeTitle(outerItem.Label)
                .InvokeSnippet(outerItem.Address)
                .InvokeSnippet(outerItem.Address)
                .Draggable(outerItem.IsDraggable)
                .InvokeRotation(outerItem.Rotation)
                .Anchor((float)outerItem.Anchor.X, (float)outerItem.Anchor.Y)
                .InvokeZIndex(outerItem.ZIndex)
                .Flat(outerItem.Flat)
                .InvokeAlpha(1f - outerItem.Transparency);

            if (outerItem.Icon != null)
            {
                var factory = _bitmapDescriptorFactory ?? DefaultBitmapDescriptorFactory.Instance;
                var nativeDescriptor = factory.ToNative(outerItem.Icon);
                opts.InvokeIcon(nativeDescriptor);
            }

            _onMarkerCreating(outerItem, opts);

            var marker = NativeMap.AddMarker(opts);
            // If the pin has an IconView set this method will convert it into an icon for the marker
            if (outerItem?.Icon?.Type == BitmapDescriptorType.View)
            {
                marker.Visible = false; // Will become visible once the iconview is ready.
                TransformXamarinViewToAndroidBitmap(outerItem, marker);
            }
            else
            {
                marker.Visible = outerItem.IsVisible;
            }

            // associate pin with marker for later lookup in event handlers
            outerItem.NativeObject = marker;
            _onMarkerCreated(outerItem, marker);
            return marker;
        }

        protected override Marker DeleteNativeItem(Pin outerItem)
        {
            var marker = outerItem.NativeObject as Marker;
            if (marker == null)
                return null;
            _onMarkerDeleting(outerItem, marker);
            marker.Remove();
            outerItem.NativeObject = null;

            if (ReferenceEquals(Map.SelectedPin, outerItem))
                Map.SelectedPin = null;

            _onMarkerDeleted(outerItem, marker);
            return marker;
        }

        Pin LookupPin(Marker marker)
        {
            return GetItems(Map).FirstOrDefault(outerItem => ((Marker)outerItem.NativeObject).Id == marker.Id);
        }

        void OnInfoWindowClick(object sender, HuaweiMap.InfoWindowClickEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.P0);

            // only consider event handled if a handler is present.
            // Else allow default behavior of displaying an info window.
            targetPin?.SendTap();

            if (targetPin != null)
            {
                Map.SendInfoWindowClicked(targetPin);
            }
        }

        private void OnInfoWindowLongClick(object sender, HuaweiMap.InfoWindowLongClickEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.P0);

            // only consider event handled if a handler is present.
            // Else allow default behavior of displaying an info window.
            if (targetPin != null)
            {
                Map.SendInfoWindowLongClicked(targetPin);
            }
        }

        void OnMakerClick(object sender, HuaweiMap.MarkerClickEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.P0);

            // If set to PinClickedEventArgs.Handled = true in app codes,
            // then all pin selection controlling by app.
            if (Map.SendPinClicked(targetPin))
            {
                e.Handled = true;
                return;
            }

            try
            {
                _onMarkerEvent = true;
                if (targetPin != null && !ReferenceEquals(targetPin, Map.SelectedPin))
                    Map.SelectedPin = targetPin;
            }
            finally
            {
                _onMarkerEvent = false;
            }

            e.Handled = false;
        }

        void OnInfoWindowClose(object sender, HuaweiMap.InfoWindowCloseEventArgs e)
        {
            // lookup pin
            var targetPin = LookupPin(e.P0);

            try
            {
                _onMarkerEvent = true;
                if (targetPin != null && ReferenceEquals(targetPin, Map.SelectedPin))
                    Map.SelectedPin = null;
            }
            finally
            {
                _onMarkerEvent = false;
            }
        }

        void OnMarkerDragStart(object sender, HuaweiMap.MarkerDragStartEventArgs e)
        {
            // lookup pin
            _draggingPin = LookupPin(e.P0);

            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.P0.Position.ToPosition());
                Map.SendPinDragStart(_draggingPin);
            }
        }

        void OnMarkerDrag(object sender, HuaweiMap.MarkerDragEventArgs e)
        {
            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.P0.Position.ToPosition());
                Map.SendPinDragging(_draggingPin);
            }
        }

        void OnMarkerDragEnd(object sender, HuaweiMap.MarkerDragEndEventArgs e)
        {
            if (_draggingPin != null)
            {
                UpdatePositionWithoutMove(_draggingPin, e.P0.Position.ToPosition());
                Map.SendPinDragEnd(_draggingPin);
                _draggingPin = null;
            }
            _withoutUpdateNative = false;
        }

        public override void OnMapPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Map.SelectedPinProperty.PropertyName)
            {
                if (!_onMarkerEvent)
                    UpdateSelectedPin(Map.SelectedPin);
                Map.SendSelectedPinChanged(Map.SelectedPin);
            }
        }

        void UpdateSelectedPin(Pin pin)
        {
            if (pin == null)
            {
                foreach (var outerItem in GetItems(Map))
                {
                    (outerItem.NativeObject as Marker)?.HideInfoWindow();
                }
            }
            else
            {
                // lookup pin
                var targetPin = LookupPin(pin.NativeObject as Marker);
                (targetPin?.NativeObject as Marker)?.ShowInfoWindow();
            }
        }

        void UpdatePositionWithoutMove(Pin pin, Position position)
        {
            try
            {
                _withoutUpdateNative = true;
                pin.Position = position;
            }
            finally
            {
                _withoutUpdateNative = false;
            }
        }

        protected override void OnUpdateAddress(Pin outerItem, Marker nativeItem)
            => nativeItem.Snippet = outerItem.Address;

        protected override void OnUpdateLabel(Pin outerItem, Marker nativeItem)
            => nativeItem.Title = outerItem.Label;

        protected override void OnUpdatePosition(Pin outerItem, Marker nativeItem)
        {
            if (!_withoutUpdateNative)
            {
                nativeItem.Position = outerItem.Position.ToLatLng();
            }
        }

        protected override void OnUpdateType(Pin outerItem, Marker nativeItem)
        {
        }

        protected override void OnUpdateIcon(Pin outerItem, Marker nativeItem)
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
                nativeItem.SetIcon(nativeDescriptor);
            }
        }

        private async void TransformXamarinViewToAndroidBitmap(Pin outerItem, Marker nativeItem)
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

                var icon = await Utils.ConvertViewToBitmapDescriptor(otherView);
                if (outerItem.NativeObject != null)
                {
                    nativeItem.SetIcon(icon);
                    nativeItem.SetAnchor((float)iconView.AnchorX, (float)iconView.AnchorY);
                    nativeItem.Visible = true;
                }
            }
        }

        protected override void OnUpdateIsDraggable(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Draggable = outerItem?.IsDraggable ?? false;
        }

        protected override void OnUpdateRotation(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Rotation = outerItem?.Rotation ?? 0f;
        }

        protected override void OnUpdateIsVisible(Pin outerItem, Marker nativeItem)
        {
            var isVisible = outerItem?.IsVisible ?? false;
            nativeItem.Visible = isVisible;

            if (!isVisible && ReferenceEquals(Map.SelectedPin, outerItem))
            {
                Map.SelectedPin = null;
            }
        }
        protected override void OnUpdateAnchor(Pin outerItem, Marker nativeItem)
        {
            nativeItem.SetAnchor((float)outerItem.Anchor.X, (float)outerItem.Anchor.Y);
        }

        protected override void OnUpdateFlat(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Flat = outerItem.Flat;
        }

        protected override void OnUpdateInfoWindowAnchor(Pin outerItem, Marker nativeItem)
        {
            nativeItem.SetInfoWindowAnchor((float) outerItem.InfoWindowAnchor.X, (float) outerItem.InfoWindowAnchor.Y);
        }

        protected override void OnUpdateZIndex(Pin outerItem, Marker nativeItem)
        {
            nativeItem.ZIndex = outerItem.ZIndex;
        }

        protected override void OnUpdateTransparency(Pin outerItem, Marker nativeItem)
        {
            nativeItem.Alpha = 1f - outerItem.Transparency;
        }
    }
}

