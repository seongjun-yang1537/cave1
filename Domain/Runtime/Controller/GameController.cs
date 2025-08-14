using Corelib.Utils;
using UnityEngine;
using VContainer;

namespace Domain
{
    [DefaultExecutionOrder(-100)]
    public class GameController : Singleton<GameController>
    {
        [Inject]
        public GameModel gameModel;

        [LifecycleInject]
        public GamePlayerController player;
        public static GamePlayerController Player => Instance.player;

        [LifecycleInject]
        public GameTimeController time;
        public static GameTimeController Time => Instance.time;

        [LifecycleInject]
        public GameQuestController quest;
        public static GameQuestController Quest => Instance.quest;

        protected virtual void Awake()
        {
            LifecycleInjectionUtil.ConstructLifecycleObjects(this);
        }
        protected virtual void OnEnable()
        {
            LifecycleInjectionUtil.OnEnable(this);
        }
        protected virtual void Start()
        {
            LifecycleInjectionUtil.Start(this);
        }
        protected virtual void OnDisable()
        {
            LifecycleInjectionUtil.OnDisable(this);
        }
    }
}