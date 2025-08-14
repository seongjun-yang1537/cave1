using Corelib.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class ItemDropAnimation : IInitializable, ITickable
    {
        [Inject] private Transform transform;
        private Transform body;

        public float rotateSpeed = 30f;
        public float hoverHeight = 0.1f;
        public float hoverSpeed = 2.5f;

        public void Initialize()
        {
            body = transform.FindInChild(nameof(body));
        }

        public void Tick()
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            transform.eulerAngles += Vector3.up * rotateSpeed * Time.deltaTime;

            float hoverOffset = (Mathf.Sin(Time.time * hoverSpeed) + 1) / 2 * hoverHeight;
            Vector3 offset = new Vector3(0, hoverOffset, 0);
            body.localPosition = offset;
        }
    }
}

