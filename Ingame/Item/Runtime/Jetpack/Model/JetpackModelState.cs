using System;
using Core;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "New Jetpack Model Data", menuName = "Game/Player/Jetpack Model Data")]
    public class JetpackModelState : InstanceScriptableObject
    {
        public float fuel;
        public JetpackState state;

        public override object CreateInstance()
        {
            return null;
        }
    }
}
