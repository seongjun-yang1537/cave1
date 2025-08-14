using Corelib.Utils;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(ParentController))]
    public class ParentController : PawnController
    {
        public ParentModel parentModel { get; private set; }
        public ParentView parentView;

        protected override void Awake()
        {
            base.Awake();
            parentModel = (ParentModel)pawnModel;
            parentView = (ParentView)pawnView;
        }
    }
}