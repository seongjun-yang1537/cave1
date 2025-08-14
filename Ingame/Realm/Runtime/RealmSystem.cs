using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Corelib.Utils;
using Cysharp.Threading.Tasks;
using Ingame;
using UnityEngine;
using UnityEngine.Events;
using World;

namespace Realm
{
    public class RealmSystem : Singleton<RealmSystem>
    {
        [NonSerialized]
        public UnityEvent<float> onProgressTeleport = new();

        public RealmGenerationProfile generationProfile;
        public bool isTestbed;

        [SerializeField]
        public RealmModel model;

        protected virtual void OnEnable()
        {
            Rebuild();
            OBJHatchController.onTeleport.AddListener(OnDescend);
        }

        protected virtual void OnDisable()
        {
            OBJHatchController.onTeleport.RemoveListener(OnDescend);
        }

        private async void OnDescend()
        {
            await DescendWorld();
        }

        public static async UniTask DescendWorld()
        {
            if (Instance.generationProfile == null) return;

            float depth = Instance.model.depth;
            RealmProfile profile = Instance.generationProfile[depth];
            if (profile == null) return;

            float newDepth = depth + profile.nextDepthRange.Sample(GameRng.World);

            await Rebuild(newDepth);
        }

        public static async UniTask Rebuild(float depth = 0)
        {
            if (Instance.generationProfile == null) return;

            WorldGenerationPreset preset;
            if (Instance.isTestbed)
            {
                preset = Instance.generationProfile.testbedPreset;
            }
            else
            {
                RealmProfile profile = Instance.generationProfile[depth];
                if (profile == null) return;
                preset = profile.generationPreset;
            }

            if (preset == null) return;

            Instance.model.SetDepth(depth);
            await GameWorld.GenerateWorld(preset);

            RewardItemPlayer(depth);

            RealmDepthConfig depthConfig = Instance.model.depthConfig;
            PlayerController playerController = EntitySystem.Players.FirstOrDefault();
            GameActionEventBus.Publish(GameActionType.PlayerReachedDepth, $"{{\"playerId\":{playerController.playerModel.entityID},\"depth\":{depth}}}");
            playerController.SetOxygenPerSecond(depthConfig.oxygenConsumptionPerSecond);
        }

        private static void RewardItemPlayer(float depth)
        {
            RealmConfigTable table = Instance.model.configTable;
            if (table == null) return;

            PlayerController playerController = PlayerSystem.CurrentPlayer;
            List<ItemModel> itemModels = table.GetRewardByDepth(depth);
            foreach (var itemModel in itemModels)
            {
                playerController.AcquireItem(itemModel);
            }
        }
    }
}