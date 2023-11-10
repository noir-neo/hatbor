using UnityEngine;

namespace Hatbor.Rig
{
    public interface ICameraRig
    {
        bool Enabled { get; }
        int Order { get; }
        void Update(Camera camera);
    }
}