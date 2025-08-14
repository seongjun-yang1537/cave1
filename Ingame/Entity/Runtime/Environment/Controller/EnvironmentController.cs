using Corelib.Utils;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(EnvironmentScope))]
    public class EnvironmentController : EntityController
    {
        public EnvironmentModel environmentModel { get; private set; }
        public EnvironmentView environmentView;

        protected void Awake()
        {
            base.Awake();
            environmentModel = (EnvironmentModel)entityModel;
            environmentView = (EnvironmentView)entityView;
        }
    }
}