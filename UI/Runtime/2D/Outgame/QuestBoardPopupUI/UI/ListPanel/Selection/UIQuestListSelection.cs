using Corelib.Utils;
using DG.Tweening;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIQuestListSelection : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private Image imgSelection;

        [Group("Placeholder"), SerializeField]
        private UIQuestBoardElement targetElement;

        public override void Render()
        {
            imgSelection.gameObject.SetActive(targetElement != null);
            if (targetElement == null) return;
        }

        public void Render(UIQuestBoardElement element)
        {
            if (this.targetElement != element)
                this.targetElement = element;
            else
                this.targetElement = null;
            Render();
        }

        protected virtual void Update()
        {
            if (targetElement == null) return;
            Vector3 nowPosition = imgSelection.transform.position;
            Vector3 targetPosition = targetElement.transform.position;
            imgSelection.transform.position = Vector3.Lerp(nowPosition, targetPosition, 0.9f);
        }
    }
}