using System;
using UnityEngine;
using UnityEngine.Events;

namespace Realm
{
    [Serializable]
    public class RealmModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        [NonSerialized]
        public readonly UnityEvent<float> onDepth = new();

        [field: SerializeField]
        public float depth { get; private set; } = 0f;

        public RealmConfigTable configTable => data.configTable;
        public RealmDepthConfig depthConfig => configTable.GetConfigByDepth(depth);

        private readonly RealmModelData data;
        public RealmModelData Data => data;

        public RealmModel(RealmModelData data)
        {
            this.data = data;
        }

        public void SetDepth(float newDepth)
        {
            this.depth = newDepth;
            onDepth.Invoke(depth);
        }
    }
}