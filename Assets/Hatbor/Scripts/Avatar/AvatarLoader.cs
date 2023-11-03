using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Avatar
{
    public sealed class AvatarLoader : IStartable, IDisposable
    {
        readonly LifetimeScope currentScope;
        readonly AvatarSettings settings;
        readonly CompositeDisposable avatarDisposables = new();
        readonly CompositeDisposable disposables = new();

        [Inject]
        public AvatarLoader(LifetimeScope lifetimeScope, AvatarSettings settings)
        {
            currentScope = lifetimeScope;
            this.settings = settings;

            avatarDisposables.AddTo(disposables);
        }

        void IStartable.Start()
        {
            settings.Path
                .Subscribe(Load)
                .AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        void Load(string path)
        {
            avatarDisposables.Clear();
            var avatarInstaller = new AvatarInstaller(path);
            currentScope.CreateChild(avatarInstaller)
                .AddTo(avatarDisposables);
        }
    }
}
