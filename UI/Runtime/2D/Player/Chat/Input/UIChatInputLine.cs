using System.Collections.Generic;
using Corelib.Utils;
using TMPro;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIChatInputLine : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private TMP_InputField txtInput;

        protected override void OnEnable()
        {
            base.OnEnable();
            txtInput.onSubmit.AddListener(OnSubmit);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            txtInput.onSubmit.RemoveListener(OnSubmit);
        }

        private void OnSubmit(string text)
        {
            viewHandler.SendEventBus(new UIChatInputLineEventBus()
            {
                inputValue = text,
            });
            txtInput.text = string.Empty;
            txtInput.ActivateInputField();
        }

        public void Focus()
            => txtInput.ActivateInputField();

        public override void Render()
        {
        }
    }
}