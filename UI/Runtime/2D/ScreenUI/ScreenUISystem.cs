using Corelib.Utils;
using TriInspector;
using UI;
using UnityEngine;
using UnityEngine.Events;
using World;

namespace Ingame
{
    public class ScreenUISystem : Singleton<ScreenUISystem>
    {
        [field: SerializeField]
        public float Fade { get; private set; }

        [Required, ReferenceBind, SerializeField]
        private UIScreenInteraction uiScreenInteraction;
        [Required, ReferenceBind, SerializeField]
        private UIScreenDay uiScreenDay;

        private UIScreenFade screenFade;
        private UIScreenTint screenTint;
        private UIScreenFlash screenFlash;
        private UIScreenLetterbox letterbox;
        private UIWorldgenProgress worldgenProgress;
        private UIPlayerDamaged playerDamaged;

        protected virtual void Awake()
        {
            screenFade = GetComponentInChildren<UIScreenFade>();
            screenTint = GetComponentInChildren<UIScreenTint>();
            screenFlash = GetComponentInChildren<UIScreenFlash>();
            letterbox = GetComponentInChildren<UIScreenLetterbox>();
            worldgenProgress = GetComponentInChildren<UIWorldgenProgress>();
            playerDamaged = GetComponentInChildren<UIPlayerDamaged>();

            uiScreenInteraction.Render("");
        }

        protected virtual void OnEnable()
        {
            EntitySystem.OnSpawnPlayer.AddListener(OnSpawnPlayer);
        }

        protected virtual void OnDisable()
        {
            EntitySystem.OnSpawnPlayer.RemoveListener(OnSpawnPlayer);
        }

        private void OnSpawnPlayer(PlayerController playerController)
        {
            PlayerView playerView = playerController.playerView;
            playerView.onTakeDamage.AddListener((_, _, _) => TriggerPlayerDamaged());
        }

        // protected virtual void OnEnable()
        // {
        //     if (GameWorld.Instance != null)
        //         GameWorld.Instance.onPipelineStart.AddListener(RegisterPipeline);
        // }

        // protected virtual void OnDisable()
        // {
        //     if (GameWorld.Instance != null)
        //         GameWorld.Instance.onPipelineStart.RemoveListener(RegisterPipeline);
        // }

        public static void ShowInteractionUI(string description = "")
        {
            Instance.uiScreenInteraction.Render(description);
        }

        public static void TriggerDayUI(int day)
        {
            Instance.uiScreenDay.Render(day);
        }

        public static void RegisterOnNextDayUIEndOnce(UnityAction callback)
        {
            Instance.uiScreenDay.onRenderEnd.AddListenerOnce(callback);
        }

        private void RegisterPipeline(WorldgenPipeline pipeline)
        {
            worldgenProgress.OnPipelineStart();
            pipeline.onStepStarted.AddListener(step => worldgenProgress.OnStepStarted(step));
        }

        public void SetFade(float newFade)
        {
            Fade = newFade;
            screenFade.SetFade(Fade);
        }

        public void SetTint(Color color, float duration = 0.25f)
        {
            screenTint?.SetTint(color, duration);
        }

        public void Flash(Color color, float duration = 0.2f)
        {
            screenFlash?.Flash(color, duration);
        }

        public void SetLetterbox(bool visible, float duration = 0.5f)
        {
            letterbox?.Toggle(visible, duration);
        }

        public void TriggerPlayerDamaged()
        {
            playerDamaged.TriggerDamaged();
        }
    }
}