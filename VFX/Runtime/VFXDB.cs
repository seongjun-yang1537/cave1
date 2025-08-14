using System.Collections.Generic;
using System.Net.Security;
using Corelib.Utils;
using UnityEngine;

namespace VFX
{
    public static class VFXDB
    {
        private const string PATH_VFX_PREFIX = "VFX";

        private static Dictionary<VFXID, GameObject> vfxs = new();

        public static GameObject GetVFX(VFXID key)
        {
            if (!vfxs.ContainsKey(key))
                vfxs.Add(key, Resources.Load<GameObject>($"{PATH_VFX_PREFIX}/VFX_{key}"));

            return vfxs[key];
        }
    }
}