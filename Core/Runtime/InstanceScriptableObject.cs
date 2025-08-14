using UnityEngine;

namespace Core
{
    public abstract class InstanceScriptableObject : ScriptableObject
    {
        public abstract object CreateInstance();
    }
}
