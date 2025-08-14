using Corelib.Utils;
using UnityEngine;

namespace VFX
{
    public class VFXSystem : Singleton<VFXSystem>
    {
        public static VFXController SpawnVFX(VFXSpawnContext context)
        {
            GameObject go = Instantiate(VFXDB.GetVFX(context.vfxID));
            go.name = $"[HeldVFX]{context.vfxID}";

            Transform tr = go.transform;
            tr.SetParent(Instance.transform);
            tr.position = context.position;

            VFXController controller = go.GetComponent<VFXController>();
            controller.SetDuration(context.duration);

            return controller;
        }
    }

    public struct VFXSpawnContext
    {
        public Vector3 position;
        public VFXID vfxID;
        public float duration;

        public VFXSpawnContext(Vector3 position, VFXID vfxID, float duration = float.MaxValue)
        {
            this.position = position;
            this.vfxID = vfxID;
            this.duration = duration;
        }

        public static VFXSpawnContextBuilder Builder() => new VFXSpawnContextBuilder();
    }

    public class VFXSpawnContextBuilder
    {
        private Vector3 _position;
        private VFXID _vfxID;
        private float _duration = float.MaxValue;

        private bool _hasPosition = false;
        private bool _hasVFXID = false;

        public VFXSpawnContextBuilder SetPosition(Vector3 position)
        {
            _position = position;
            _hasPosition = true;
            return this;
        }

        public VFXSpawnContextBuilder SetVFXID(VFXID vfxID)
        {
            _vfxID = vfxID;
            _hasVFXID = true;
            return this;
        }

        public VFXSpawnContextBuilder SetDuration(float duration)
        {
            _duration = duration;
            return this;
        }

        public VFXSpawnContext Build()
        {
            if (!_hasPosition)
                throw new System.InvalidOperationException("VFXSpawnContextBuilder: position is required.");
            if (!_hasVFXID)
                throw new System.InvalidOperationException("VFXSpawnContextBuilder: vfxID is required.");

            return new VFXSpawnContext(_position, _vfxID, _duration);
        }
    }
}