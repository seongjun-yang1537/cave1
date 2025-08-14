using System;
using Corelib.Utils;
using UnityEngine;

namespace UI
{
    public class PopupContext
    {
        public Func<ControllerBaseBehaviour, ControllerBaseBehaviour> PrefabModifier { get; private set; }
        public Func<ControllerBaseBehaviour, ControllerBaseBehaviour> CreatedHandler { get; private set; }
        public Func<ControllerBaseBehaviour, ControllerBaseBehaviour> PreDestroyHandler { get; private set; }
        public class Builder
        {
            private readonly PopupContext context = new PopupContext();
            public Builder SetPrefabModifier(Func<ControllerBaseBehaviour, ControllerBaseBehaviour> value)
            {
                context.PrefabModifier = value;
                return this;
            }
            public Builder SetCreatedHandler(Func<ControllerBaseBehaviour, ControllerBaseBehaviour> value)
            {
                context.CreatedHandler = value;
                return this;
            }
            public Builder SetPreDestroyHandler(Func<ControllerBaseBehaviour, ControllerBaseBehaviour> value)
            {
                context.PreDestroyHandler = value;
                return this;
            }
            public PopupContext Build() { return context; }
        }
        public static Builder Create() { return new Builder(); }
    }
}
