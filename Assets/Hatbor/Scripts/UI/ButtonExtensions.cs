using System;
using UniRx;
using UnityEngine.UIElements;

namespace Hatbor.UI
{
    public static class ButtonExtensions
    {
        public static IObservable<Unit> OnClickAsObservable(this Button button)
        {
            return Observable.FromEvent(
                h => button.clickable.clicked += h,
                h => button.clickable.clicked -= h);
        }
    }
}