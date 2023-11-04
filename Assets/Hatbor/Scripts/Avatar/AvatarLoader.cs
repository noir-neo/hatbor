using System;
using Hatbor.Config;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Hatbor.Avatar
{
    public sealed class AvatarLoader : IStartable, IDisposable
    {
        readonly LifetimeScope currentScope;
        readonly AvatarConfig config;
        readonly CompositeDisposable avatarDisposables = new();
        readonly CompositeDisposable disposables = new();

        [Inject]
        public AvatarLoader(LifetimeScope lifetimeScope, AvatarConfig config)
        {
            currentScope = lifetimeScope;
            this.config = config;

            avatarDisposables.AddTo(disposables);
        }

        void IStartable.Start()
        {
            config.Path
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
