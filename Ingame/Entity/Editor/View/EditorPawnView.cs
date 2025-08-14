using Corelib.SUI;
using Corelib.Utils;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    [CustomEditor(typeof(PawnView), true)]
    public class EditorPawnView : EditorAgentView
    {
        PawnView pawnView;

        Transform hand;

        protected override void OnEnable()
        {
            base.OnEnable();
            pawnView = (PawnView)target;
        }

        protected override SUIElement DrawValidationElement()
        {
            return SEditorGUILayout.Vertical()
            .Content(
                base.DrawValidationElement()
                + EditorValidationHelper.CreateValidationField(nameof(hand), hand, pawnView.transform, UpdateReference)
            );
        }

        protected override void UpdateReference()
        {
            base.UpdateReference();
            hand = (target as PawnView).transform.FindInChild(nameof(hand));
        }

        protected override bool CheckValidation()
        {
            if (!base.CheckValidation())
                return false;

            if (hand == null || hand.GetComponent<HandController>() == null)
                return false;

            return true;
        }
    }
}