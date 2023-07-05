using System;
namespace Xamarin.Forms.GoogleMaps.Android.Logics
{
    public class DelegateCancelableCallback : Java.Lang.Object, global::Huawei.Hms.Maps.HuaweiMap.ICancelableCallback
    {
        private readonly Action _onFinish;
        private readonly Action _onCancel;

        public DelegateCancelableCallback(Action onFinish, Action onCancel)
        {
            _onFinish = onFinish;
            _onCancel = onCancel;
        }

        public void OnFinish()
        {
            _onFinish?.Invoke();
        }

        public void OnCancel()
        {
            _onCancel?.Invoke();
        }
    }
}
