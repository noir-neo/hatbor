using System;
using Hatbor.Config;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Hatbor.UI
{
    public sealed class LegalNoticesRoot : IStartable, IDisposable
    {
        readonly UIDocument uiDocument;
        readonly MiscConfig config;

        readonly CompositeDisposable disposables = new();

        [Inject]
        public LegalNoticesRoot(UIDocument uiDocument, MiscConfig config)
        {
            this.uiDocument = uiDocument;
            this.config = config;
        }

        void IStartable.Start()
        {
            config.LegalNotices
                .Subscribe(enabled => uiDocument.enabled = enabled)
                .AddTo(disposables);

            uiDocument.ObserveEveryValueChanged(x => x.enabled)
                // NOTE: Caching rootVisualElement or CloseButton is not possible because uiDocument may become disabled.
                .Select(enabled => enabled
                    ? uiDocument.rootVisualElement.Q<Button>("CloseButton").OnClickAsObservable()
                    : Observable.Empty<Unit>())
                .Switch()
                .Subscribe(_ => config.LegalNotices.Value = false)
                .AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}