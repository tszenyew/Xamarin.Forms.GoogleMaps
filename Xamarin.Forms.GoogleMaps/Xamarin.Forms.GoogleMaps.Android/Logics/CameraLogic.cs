using System;
using Huawei.Hms.Maps;
using Huawei.Hms.Maps.Model;
using Java.Lang;
using Xamarin.Forms.GoogleMaps.Android.Extensions;
using Xamarin.Forms.GoogleMaps.Android.Logics;
using Xamarin.Forms.GoogleMaps.Internals;
using static Huawei.Hms.Maps.HuaweiMap;
using GCameraPosition = Huawei.Hms.Maps.Model.CameraPosition;

using GCameraUpdateFactory = Huawei.Hms.Maps.CameraUpdateFactory;

namespace Xamarin.Forms.GoogleMaps.Logics.Android
{
    public sealed class CameraLogic : BaseCameraLogic<HuaweiMap>
    {
        private readonly Action<LatLng> _updateVisibleRegion;

        public CameraLogic(Action<LatLng> updateVisibleRegion)
        {
            _updateVisibleRegion = updateVisibleRegion;
        }

        public override void Register(Map map, HuaweiMap nativeMap)
        {
            base.Register(map, nativeMap);

            UnsubscribeCameraEvents(_nativeMap);

            nativeMap.CameraMoveStarted += NativeMap_CameraMoveStarted;
            nativeMap.CameraMove += NativeMap_CameraMove;
            nativeMap.CameraIdle += NativeMap_CameraIdle;
        }

        public override void Unregister()
        {
            UnsubscribeCameraEvents(_nativeMap);
            base.Unregister();
        }

        private void UnsubscribeCameraEvents(HuaweiMap nativeMap)
        {
            if (nativeMap == null)
            {
                return;
            }
            
            nativeMap.CameraMoveStarted -= NativeMap_CameraMoveStarted;
            nativeMap.CameraMove -= NativeMap_CameraMove;
            nativeMap.CameraIdle -= NativeMap_CameraIdle;
        }

        public override void OnMoveToRegionRequest(MoveToRegionMessage m)
        {
            if (_nativeMap == null)
                return;

            var span = m.Span;
            var animate = m.Animate;

            span = span.ClampLatitude(85, -85);
            var ne = new LatLng(span.Center.Latitude + span.LatitudeDegrees / 2, span.Center.Longitude + span.LongitudeDegrees / 2);
            var sw = new LatLng(span.Center.Latitude - span.LatitudeDegrees / 2, span.Center.Longitude - span.LongitudeDegrees / 2);
            var update = GCameraUpdateFactory.NewLatLngBounds(new LatLngBounds(sw, ne), 0);

            try
            {
                if (animate)
                    _nativeMap.AnimateCamera(update);
                else
                    _nativeMap.MoveCamera(update);
            }
            catch (IllegalStateException exc)
            {
                System.Diagnostics.Debug.WriteLine("MoveToRegion exception: " + exc);
            }
        }

        public override void OnMoveCameraRequest(CameraUpdateMessage m)
        {
            MoveCamera(m.Update);
            m.Callback.OnFinished();
        }

        internal void MoveCamera(CameraUpdate update)
        {
            _nativeMap.MoveCamera(update.ToAndroid(ScaledDensity));
        }

        public override void OnAnimateCameraRequest(CameraUpdateMessage m)
        {
            var update = m.Update.ToAndroid(ScaledDensity);
            var callback = new DelegateCancelableCallback(
                    () => m.Callback.OnFinished(),
                    () => m.Callback.OnCanceled());

            if (m.Duration.HasValue)
            {
                _nativeMap.AnimateCamera(update, (int)m.Duration.Value.TotalMilliseconds, callback);
            }
            else
            {
                _nativeMap.AnimateCamera(update, callback);
            }
        }


        void NativeMap_CameraMoveStarted(object sender, HuaweiMap.CameraMoveStartedEventArgs e)
        {
            // see https://developers.google.com/maps/documentation/android-api/events#camera_change_events
            _map.SendCameraMoveStarted(e.P0 == OnCameraMoveStartedListener.ReasonGesture);
        }

        void NativeMap_CameraMove(object sender, System.EventArgs e)
        {
            _map.SendCameraMoving(_nativeMap.CameraPosition.ToXamarinForms());
        }

        void NativeMap_CameraIdle(object sender, System.EventArgs e)
        {
            _map.SendCameraIdled(_nativeMap.CameraPosition.ToXamarinForms());
        }
    }
}