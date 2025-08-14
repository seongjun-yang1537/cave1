using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [RequireComponent(typeof(OBJHatchScope))]
    public class OBJHatchController : EntityController
    {
        public static UnityEvent<float> onProgressTeleport = new();
        public static UnityEvent onTeleport = new();

        public OBJHatchModel objHatchModel { get; private set; }
        public OBJHatchView objHatchView;

        public float teleportingDuration = 5.0f;
        float progressTeleporting = 0.0f;

        protected void Awake()
        {
            base.Awake();
            objHatchModel = (OBJHatchModel)entityModel;
            objHatchView = (OBJHatchView)entityView;

            onProgressTeleport.AddListener(v => Debug.Log(v));
        }

        protected virtual void OnTriggerEnter(Collider other)
        {

        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (progressTeleporting >= teleportingDuration)
            {
                Debug.Log("teleport");
                onTeleport.Invoke();
            }

            progressTeleporting += Time.deltaTime;
            onProgressTeleport.Invoke(progressTeleporting);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            progressTeleporting = 0f;
            onProgressTeleport.Invoke(progressTeleporting);
        }
    }
}