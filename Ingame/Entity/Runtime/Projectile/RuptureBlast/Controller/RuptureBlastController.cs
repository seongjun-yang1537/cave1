using Corelib.Utils;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(RuptureBlastScope))]
    public class RuptureBlastController : ProjectileController
    {
        public RuptureBlastModel ruptureBlastModel { get; private set; }
        public RuptureBlastView ruptureBlastView;

        protected override void Awake()
        {
            base.Awake();
            ruptureBlastModel = (RuptureBlastModel)projectileModel;
            ruptureBlastView = (RuptureBlastView)projectileView;
        }

        protected override void Update()
        {
            base.Update();
            foreach (var player in EntitySystem.Players)
            {
                float sqrDist = (player.transform.position - transform.position).sqrMagnitude;
                float sqrRange = ruptureBlastModel.range * ruptureBlastModel.range;
                if (sqrDist < sqrRange)
                {
                    Hit(player.GetComponent<EntityController>());
                }
            }
        }
    }
}