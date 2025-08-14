using Corelib.Utils;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(NPCScope))]
    public class NPCController : PawnController
    {
        public NPCModel npcModel { get; private set; }
        public NPCView npcView;

        protected override void Awake()
        {
            base.Awake();
            npcModel = (NPCModel)pawnModel;
            npcView = (NPCView)pawnView;
        }
    }
}