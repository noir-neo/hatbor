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

        public void Initialize(Vrm10Instance instance)
        {
            // NOTE: Runtime の生成より前に instance.transform を動かすと ControlRig がズレる (UniVRM v0.115.0)
            // ので、とりあえず初期化しておく必要あり
            var runtime = instance.Runtime;
        }

        public void Update(Vrm10Instance instance)
        {
            rootTransformRig.Update(instance.transform);
            humanoidRig.Update(instance.Runtime.ControlRig);
            expressionRig.Update(instance.Runtime.Expression);
        }
    }
}