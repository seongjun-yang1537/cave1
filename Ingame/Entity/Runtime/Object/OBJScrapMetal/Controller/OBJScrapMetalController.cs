using Corelib.Utils;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(OBJScrapMetalScope))]
    public class OBJScrapMetalController : EntityController
    {
        public OBJScrapMetalModel objScrapMetalModel { get; private set; }
        public OBJScrapMetalView objScrapMetalView;

        protected void Awake()
        {
            base.Awake();
            objScrapMetalModel = (OBJScrapMetalModel)entityModel;
            objScrapMetalView = (OBJScrapMetalView)entityView;
        }
    }
}