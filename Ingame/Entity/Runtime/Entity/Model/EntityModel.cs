using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathX;

namespace Ingame
{
    [Serializable]
    public class EntityModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent onSpawn = new();
        [NonSerialized]
        public readonly UnityEvent onDead = new();
        #endregion ====================

        #region ========== Data ==========
        private readonly EntityModelData data;
        public EntityType entityType => data.entityType;
        public TriangleDomain navDomain => data.navDomain;
        public EntityModelData Data => data;
        #endregion

        #region ========== State ==========
        public bool isSpanwed = true;

        [field: SerializeField]
        public int entityID { get; private set; }
        #endregion ====================

        protected EntityModel(EntityModelData data, EntityModelState state = null)
        {
            this.data = data;

            isSpanwed = true;
            entityID = EntityIDGenerator.Generate();

            if (state != null)
            {
                isSpanwed = state.isSpanwed;
            }
        }

        protected EntityModel(EntityModel other)
        {
            data = other.data;
            isSpanwed = other.isSpanwed;
            entityID = other.entityID;
        }
        public void Dead()
        {

        }
    }
}
