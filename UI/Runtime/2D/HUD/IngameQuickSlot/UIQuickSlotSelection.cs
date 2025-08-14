using DG.Tweening;
using TriInspector;
using UnityEngine;

namespace UI
{
    [DeclareBoxGroup("Placeholder")]
    public class UIQuickSlotSelection : UIMonoBehaviour
    {
        public float moveTimer = 0.1f;

        public UIQuickSlotList uiQuickSlotList;
        private UIQuickSlotElement selectedUIInventorySlotModel;

        [Group("Placeholder")]
        public int selectedIdx;

        [Button("Render")]
        public override void Render()
        {
            uiQuickSlotList.RenderInitialize();

            selectedUIInventorySlotModel = uiQuickSlotList.elements[selectedIdx];

            if (Application.isPlaying)
                transform.DOMove(selectedUIInventorySlotModel.transform.position, moveTimer);
            else
                transform.position = selectedUIInventorySlotModel.transform.position;
        }

        public void Render(int idx)
        {
            this.selectedIdx = idx;
            Render();
        }
    }
}