using VContainer;
using VContainer.Unity;

namespace Hatbor.Avatar
{
    public sealed class AvatarInstaller : IInstaller
    {
        readonly string path;

        public AvatarInstaller(string path)
        {
            this.path = path;
        }

        void IInstaller.Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(path);
            builder.RegisterEntryPoint<Avatar>();
        }
    }
}