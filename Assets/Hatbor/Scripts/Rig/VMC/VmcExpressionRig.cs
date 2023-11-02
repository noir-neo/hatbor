using System.Linq;
using Hatbor.VMC;
using UniVRM10;
using VContainer;

namespace Hatbor.Rig.VMC
{
    public sealed class VmcExpressionRig : IExpressionRig
    {
        readonly VmcServer vmcServer;

        [Inject]
        public VmcExpressionRig(VmcServer vmcServer)
        {
            this.vmcServer = vmcServer;
        }

        void IExpressionRig.Update(Vrm10RuntimeExpression expression)
        {
            vmcServer.ProcessRead();

            var blendShapeValues = vmcServer.BlendShapeValues
                .ToDictionary(kvp => ConvertVrm0ToVrm1Preset(kvp.Key), kvp => kvp.Value);

            var expressions = expression.ExpressionKeys
                .ToDictionary(e => e, e =>
                {
                    var key = e.Preset == ExpressionPreset.custom ? e.Name : e.Preset.ToString();
                    return blendShapeValues.TryGetValue(key.ToLower(), out var value) ? value : 0f;
                });

            expression.SetWeights(expressions);
        }

        static string ConvertVrm0ToVrm1Preset(string expressionName)
        {
            return expressionName.ToLower() switch
            {
                "joy" => "happy",
                "angry" => "angry",
                "sorrow" => "sad",
                "fun" => "relaxed",
                "a" => "aa",
                "i" => "ih",
                "u" => "ou",
                "e" => "ee",
                "o" => "oh",
                "blink_l" => "blinkleft",
                "blink_r" => "blinkright",
                _ => expressionName
            };
        }
    }
}