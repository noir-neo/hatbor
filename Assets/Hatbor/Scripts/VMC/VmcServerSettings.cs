using System;
using UniRx;
using UnityEngine;

namespace Hatbor.VMC
{
    [Serializable]
    public sealed class VmcServerSettings
    {
        [SerializeField] BoolReactiveProperty enabled = new(true);
        [SerializeField] IntReactiveProperty port = new(39539);

        public ReactiveProperty<bool> Enabled => enabled;
        public ReactiveProperty<int> Port => port;
    }
}