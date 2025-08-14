using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using Quest;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [RequireComponent(typeof(PlayerQuestPopupUIView))]
    public class PlayerQuestPopupUIController : UIControllerBaseBehaviour<PlayerQuestPopupUIView>, IPopupUIController
    {
        public UnityEvent<bool> onVisible { get; } = new();
        [field: SerializeField]
        public bool visible { get; private set; }
        public bool allowMultipleInstances { get; }
        private List<QuestModel> visibleQuestModels =>
            UIServiceLocator.GameHandler.GameQuests
            .Where(q => q.phase == QuestPhase.InProgress)
            .ToList();
        protected override void OnEnable()
        {
            base.OnEnable();
            UIServiceLocator.GameHandler.OnUpdateQuest.AddListener(OnUpdateQuest);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            UIServiceLocator.GameHandler.OnUpdateQuest.RemoveListener(OnUpdateQuest);
        }
        public void Open() => SetVisible(true);
        public void Close() => SetVisible(false);
        public void Toggle() => SetVisible(!visible);
        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(bool newVisible)
        {
            view.onVisible.Invoke(this, newVisible);
            OnUpdateQuest();
        }
        public void SetVisible(bool newVisible)
        {
            visible = newVisible;
            onVisible.Invoke(newVisible);
        }
        private void OnUpdateQuest()
            => view.onUpdateQuest.Invoke(visibleQuestModels);
    }
}
