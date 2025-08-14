using UnityEngine;
using System;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class TemplateModel : ParentModel
    {
    #region ========== Event ==========

    #endregion ====================

    #region ========== Data ==========

    #endregion ====================

    #region ========== State ==========

    #endregion ====================

        public new TemplateModelData Data => base.Data as TemplateModelData;

        public TemplateModel(TemplateModelData data, TemplateModelState state = null) : base(data, state)
        {
        }
    }
}