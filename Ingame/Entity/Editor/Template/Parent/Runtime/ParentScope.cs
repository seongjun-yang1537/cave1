using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class ParentScope : PawnScope
    {
        [HideInInspector]
        public ParentModel parentModel;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }
    }
}