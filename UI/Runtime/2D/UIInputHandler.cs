using UnityEngine;

namespace UI
{
    public class UIInputHandler : MonoBehaviour
    {
        [SerializeField] private string inventoryPopupKey = "Player/Inventory";
        [SerializeField] private string questPopupKey = "Player/QuestBoard";
        [SerializeField] private string chatPopupKey = "Player/Chat";

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.E))
                PopupUISystem.TogglePopup(inventoryPopupKey);

            if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.T))
                PopupUISystem.OpenPopup(chatPopupKey);

            if (Input.GetKeyDown(KeyCode.L))
                PopupUISystem.TogglePopup(questPopupKey);

            if (Input.GetKeyDown(KeyCode.Escape))
                PopupUISystem.ClosePopup();
        }
    }
}
