using Corelib.Utils;
using TMPro;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UI3DDaySign : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private TextMeshPro txtDay;

        [Group("Placeholder")]
        public int day;

        protected override void OnEnable()
        {
            base.OnEnable();
            UIServiceLocator.GameHandler.OnNextDay.AddListener(OnNextDay);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UIServiceLocator.GameHandler.OnNextDay.RemoveListener(OnNextDay);
        }

        private void OnNextDay(int day)
        {
            Render(day);
        }

        public override void Render()
        {
            txtDay.text = $"Day {day}";
        }

        public void Render(int day)
        {
            this.day = day;
            Render();
        }
    }
}