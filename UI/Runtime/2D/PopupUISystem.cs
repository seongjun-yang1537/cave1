using System.Collections;
using System.Collections.Generic;
using Core;
using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace UI
{
    public class PopupUISystem : Singleton<PopupUISystem>
    {
        private PopupUIControllerTable PopupUIControllerTable;
        private readonly Stack<(string key, IPopupUIController popup)> popupStack = new();
        private readonly Dictionary<string, List<IPopupUIController>> activePopups = new();
        private readonly Dictionary<string, IPopupUIController> activeSingletons = new();
        private const float DESTROY_DELAY = 0.5f;
        protected virtual void Awake()
        {
            PopupUIControllerTable = Resources.Load<PopupUIControllerTable>("UI/PopupUITable");
        }
        public static void TogglePopup(string key, PopupContext context = null)
        {
            if (Instance.activePopups.TryGetValue(key, out var list))
            {
                foreach (var p in list)
                {
                    if (p.visible)
                    {
                        ClosePopup(context);
                        return;
                    }
                }
            }
            OpenPopup(key, context);
        }
        public static void OpenPopup(string key, PopupContext context = null)
        {
            if (Instance.PopupUIControllerTable == null) return;
            var entry = Instance.PopupUIControllerTable[key];
            if (entry == null || entry.prefab == null) return;
            var prefab = entry.prefab;
            if (context?.PrefabModifier != null)
                prefab = context.PrefabModifier.Invoke(prefab);
            if (!Instance.activePopups.TryGetValue(key, out var list))
                list = Instance.activePopups[key] = new();
            if (!string.IsNullOrEmpty(entry.singletonTag) && Instance.activeSingletons.ContainsKey(entry.singletonTag))
                return;
            ControllerBaseBehaviour instance = Object.Instantiate(prefab, Instance.transform);
            IPopupUIController popup = instance as IPopupUIController;
            if (!entry.fixedSortOrder)
                instance.transform.SetAsLastSibling();
            list.Add(popup);
            Instance.popupStack.Push((key, popup));
            if (!string.IsNullOrEmpty(entry.singletonTag))
                Instance.activeSingletons[entry.singletonTag] = popup;
            popup.Open();
            if (context?.CreatedHandler != null)
                context.CreatedHandler.Invoke(instance);
            Instance.UpdateCursor();
        }
        public static void ClosePopup(PopupContext context = null)
        {
            if (Instance.popupStack.Count == 0) return;
            var (key, popup) = Instance.popupStack.Pop();
            popup.Close();
            Instance.StartCoroutine(Instance.DestroyAfterClose(key, popup, context));
            Instance.UpdateCursor();
        }
        private IEnumerator DestroyAfterClose(string key, IPopupUIController popup, PopupContext context)
        {
            yield return new WaitForSeconds(DESTROY_DELAY);
            if (activePopups.TryGetValue(key, out var list))
                list.Remove(popup);
            var entry = PopupUIControllerTable[key];
            if (entry != null && !string.IsNullOrEmpty(entry.singletonTag))
            {
                if (activeSingletons.TryGetValue(entry.singletonTag, out var p) && p == popup)
                    activeSingletons.Remove(entry.singletonTag);
            }
            if (popup != null)
            {
                if (context?.PreDestroyHandler != null)
                    context.PreDestroyHandler.Invoke((ControllerBaseBehaviour)popup);
                Destroy(((ControllerBaseBehaviour)popup).gameObject);
            }
        }
        private void UpdateCursor()
        {
            bool hasPopup = popupStack.Count > 0;
            Cursor.lockState = hasPopup ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = hasPopup;
            PlayerSystem.CurrentPlayer?.SetMoveAndViewEnabled(!hasPopup);
        }
    }
}
