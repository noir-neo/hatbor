using UniVRM10;
using VContainer;

namespace Hatbor.Rig
{
    public sealed class AvatarRig
    {
        readonly IRootTransformRig rootTransformRig;
        readonly IHumanoidRig humanoidRig;
        readonly IExpressionRig expressionRig;

        [Inject]
        public AvatarRig(IRootTransformRig rootTransformRig,
            IHumanoidRig humanoidRig,
            IExpressionRig expressionRig)
        {
            this.rootTransformRig = rootTransformRig;
            this.humanoidRig = humanoidRig;
            this.expressionRig = expressionRig;
        }

        public void Update(Vrm10Instance instance)
        {
            rootTransformRig.Update(instance.transform);
            humanoidRig.Update(instance.Runtime.ControlRig);
            expressionRig.Update(instance.Runtime.Expression);
        }
    }
}