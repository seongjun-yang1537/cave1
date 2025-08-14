using Corelib.Utils;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(ToxicSpitScope))]
    public class ToxicSpitController : ProjectileController
    {
        public ToxicSpitModel toxicSpitModel { get; private set; }
        public ToxicSpitView toxicSpitView;

        protected override void Awake()
        {
            base.Awake();
            toxicSpitModel = (ToxicSpitModel)projectileModel;
            toxicSpitView = (ToxicSpitView)projectileView;
        }
    }
}
