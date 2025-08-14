using Core;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [RequireComponent(typeof(EntityScope))]
    public class EntityView : ViewBaseBehaviour
    {
        public readonly UnityEvent<EntityController, Vector3> onPosition = new();
        public readonly UnityEvent<EntityController, Quaternion> onRotation = new();
        public readonly UnityEvent<EntityController> onDead = new();

        protected Transform body;
        public RigAnchors rigAnchors = new();

        protected override void Awake()
        {
            base.Awake();
            body = transform.FindInChild(nameof(body));
            rigAnchors.Init(body);
        }

        [AutoSubscribe(nameof(onPosition))]
        protected virtual void OnPosition(EntityController entityController, Vector3 position)
        {
            entityController.transform.position = position;
        }

        [AutoSubscribe(nameof(onRotation))]
        protected virtual void OnRotation(EntityController entityController, Quaternion rotation)
        {
            entityController.transform.rotation = rotation;
        }
    }
}
