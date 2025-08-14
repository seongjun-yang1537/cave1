using UnityEngine;

namespace Ingame
{
    public class HandController : MonoBehaviour
    {
        public Transform handlItemSocket;
        public Transform handleItemGameObject { get => handlItemSocket.GetChild(0); }

        public T Hand<T>() where T : MonoBehaviour
            => handleItemGameObject.GetComponent<T>();
    }
}