using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Hatbor.Config
{
    /// <summary>Inspectable ReactiveProperty.</summary>
    [Serializable]
    public class Vector2IntReactiveProperty : ReactiveProperty<Vector2Int>
    {
        static readonly IEqualityComparer<Vector2Int> EqualityComparerCache = new Vector2IntEqualityComparer();

        public Vector2IntReactiveProperty()
        {
        }

        public Vector2IntReactiveProperty(Vector2Int initialValue)
            : base(initialValue)
        {
        }

        protected override IEqualityComparer<Vector2Int> EqualityComparer => EqualityComparerCache;

        sealed class Vector2IntEqualityComparer : IEqualityComparer<Vector2Int>
        {
            public bool Equals(Vector2Int self, Vector2Int vector)
            {
                return self.x.Equals(vector.x) && self.y.Equals(vector.y);
            }

            public int GetHashCode(Vector2Int obj)
            {
                return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2;
            }
        }

    }
}

