using Corelib.Utils;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(TemplateScope)), RequireComponent(typeof(TemplateView))]
    public class TemplateController : ParentController
    {
        public TemplateModel templateModel { get; private set; }
        public TemplateView templateView;

        protected override void Awake()
        {
            base.Awake();
            templateModel = (TemplateModel)parentModel;
            templateView = (TemplateView)parentView;
        }
    }
}