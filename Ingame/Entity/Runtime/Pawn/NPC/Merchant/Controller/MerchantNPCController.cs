using Corelib.Utils;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(MerchantNPCScope))]
    public class MerchantNPCController : NPCController
    {
        public MerchantNPCModel merchantNPCModel { get; private set; }
        public MerchantNPCView merchantNPCView;

        protected override void Awake()
        {
            base.Awake();
            merchantNPCModel = (MerchantNPCModel)npcModel;
            merchantNPCView = (MerchantNPCView)npcView;
        }
    }
}