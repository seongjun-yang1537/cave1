using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    [RequireComponent(typeof(PlayerChatPopupUIView))]
    public class PlayerChatPopupUIController : UIControllerBaseBehaviour<PlayerChatPopupUIView>, IPopupUIController
    {
        #region ========== IPopupUIController ==========
        public UnityEvent<bool> onVisible { get; } = new();
        [field: SerializeField]
        public bool visible { get; private set; }
        public bool allowMultipleInstances { get; }
        #endregion ====================

        #region ========== State ==========
        private List<string> assistList = new();
        private int assistIndex;
        public static List<ICommandProcessor> commandProcessors { get; } = new();
        #endregion ====================

        public List<string> ChatLogs => UIServiceLocator.GameHandler.ChatLogs;

        protected override void OnEnable()
        {
            base.OnEnable();
            UIServiceLocator.GameHandler.OnUpdateChat.AddListener(OnUpdateOutput);
            OnUpdateOutput();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UIServiceLocator.GameHandler.OnUpdateChat.RemoveListener(OnUpdateOutput);
        }


        #region ========== IPopupUIController ==========
        public void Open() => SetVisible(true);
        public void Close() => SetVisible(false);
        public void SetVisible(bool newVisible)
        {
            visible = newVisible;
            onVisible.Invoke(newVisible);
            if (newVisible)
                view.FocusInput();
        }
        #endregion ====================

        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(bool newVisible)
            => view.onVisible.Invoke(newVisible);

        public override void OnReceiveEventBus(UIEventBus eventBus)
        {
            switch (eventBus)
            {
                case UIChatInputLineEventBus:
                    var bus = eventBus as UIChatInputLineEventBus;
                    Submit(bus.inputValue);
                    break;
            }
        }

        public void Submit(string input)
        {
            var text = input;
            if (string.IsNullOrWhiteSpace(text))
                return;
            if (text.StartsWith("/"))
            {
                var cmd = text.Substring(1);
                foreach (var processor in commandProcessors)
                    if (processor.OnProcessCommand(cmd))
                        return;
            }
            else
                UIServiceLocator.GameHandler.AddChatLog(text);
            assistList.Clear();
            assistIndex = 0;
        }

        #region ========== View ==========
        private void OnUpdateOutput()
             => view.onUpdateOutput.Invoke(ChatLogs);

        private void OnUpdateAssist()
            => view.onUpdateAssist.Invoke(assistList, assistIndex);
        #endregion ====================
    }
}

