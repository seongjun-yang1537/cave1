using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class TemplateScope : ParentScope
    {
        [HideInInspector]
        public TemplateModel templateModel;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }
    }
}