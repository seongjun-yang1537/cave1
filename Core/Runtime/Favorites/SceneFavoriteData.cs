using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class SceneFavoriteData : ScriptableObject
    {
        public List<string> objects = new();
        public List<string> attributeObjects = new();
    }
}
