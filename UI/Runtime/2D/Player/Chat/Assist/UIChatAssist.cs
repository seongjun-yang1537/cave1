using System.Collections.Generic;
using Corelib.Utils;
using TMPro;
using TriInspector;
using UnityEngine;
using System.Text;

namespace UI
{
    public class UIChatAssist : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtAssist;

        [Group("Placeholder"), SerializeField]
        private int selectedIndex;
        [Group("Placeholder"), SerializeField]
        private List<string> assistList;

        public override void Render()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < assistList.Count; i++)
            {
                var line = assistList[i];
                if (i == selectedIndex)
                    builder.Append("<color=yellow>").Append(line).Append("</color>");
                else
                    builder.Append(line);
                if (i < assistList.Count - 1)
                    builder.Append('\n');
            }
            txtAssist.text = builder.ToString();
        }

        public void Render(List<string> assistList, int selectedIndex)
        {
            this.assistList = assistList;
            this.selectedIndex = selectedIndex;
            Render();
        }
    }
}