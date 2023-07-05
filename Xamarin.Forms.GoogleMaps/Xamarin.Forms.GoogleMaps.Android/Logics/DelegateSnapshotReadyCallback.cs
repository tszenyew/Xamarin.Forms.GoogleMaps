using System;
using Huawei.Hms.Maps;
using Android.Graphics;

namespace Xamarin.Forms.GoogleMaps.Android.Logics
{
    public sealed class DelegateSnapshotReadyCallback : Java.Lang.Object, HuaweiMap.ISnapshotReadyCallback
    {
        private readonly Action<Bitmap> _handler;

        public DelegateSnapshotReadyCallback(Action<Bitmap> handler)
        {
            _handler = handler;
        }

        public void OnSnapshotReady(Bitmap snapshot)
        {
            _handler?.Invoke(snapshot);
        }
    }
}