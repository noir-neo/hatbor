using System.Reflection;
using UniVRM10;
using VContainer;

namespace Hatbor.Rig
{
    public sealed class AvatarRig
    {
        readonly IRootTransformRig rootTransformRig;
        readonly IHumanoidRig humanoidRig;
        readonly IExpressionRig expressionRig;

        static readonly FieldInfo EyeDirectionApplicableFieldInfo = typeof(Vrm10RuntimeExpression)
            .GetField("_eyeDirectionApplicable", BindingFlags.NonPublic | BindingFlags.Instance);

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

            // NOTE: LookAt が eye bone や expression を上書きするので粉砕する
            // TODO: VMC Protocol 送信元の VRM と異なる場合に curve map の適用を再計算すべきだが、元の視線情報も必要になる
            // runtime.Expression._eyeDirectionApplicable = null;
            EyeDirectionApplicableFieldInfo.SetValue(runtime.Expression, null);
        }

        public void Update(Vrm10Instance instance)
        {
            rootTransformRig.Update(instance);
            humanoidRig.Update(instance);
            expressionRig.Update(instance);
        }
    }
}