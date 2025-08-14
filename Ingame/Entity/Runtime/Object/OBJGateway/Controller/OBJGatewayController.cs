using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ingame
{
    [RequireComponent(typeof(OBJGatewayScope))]
    public class OBJGatewayController : EntityController
    {
        public OBJGatewayModel objGatewayModel { get; private set; }
        public OBJGatewayView objGatewayView;

        public string sceneToLoad = "Ingame";
        public float teleportingDuration = 5.0f;

        private float progressTeleporting = 0.0f;
        private bool isTeleporting = false;

        protected void Awake()
        {
            base.Awake();
            objGatewayModel = (OBJGatewayModel)entityModel;
            objGatewayView = (OBJGatewayView)entityView;
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player") || isTeleporting) return;

            progressTeleporting += Time.deltaTime;

            if (progressTeleporting >= teleportingDuration)
            {
                isTeleporting = true;
                StartCoroutine(LoadSceneCoroutine());
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            progressTeleporting = 0f;
            isTeleporting = false;
        }

        private IEnumerator LoadSceneCoroutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
