using System.Collections.Generic;
using Corelib.Utils;
using TMPro;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIChatOutputLine : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtOutput;

        [Group("Placeholder"), SerializeField]
        private string output;

        public override void Render()
        {
            txtOutput.text = output;
        }

        public void Render(List<string> outputList)
        {
            this.output = outputList.ToString("\n");
            Render();
        }
    }
}