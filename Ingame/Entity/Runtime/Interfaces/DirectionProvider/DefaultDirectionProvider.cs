using UnityEngine;
using VContainer;

namespace Ingame
{
    public class DefaultDirectionProvider : IDirectionProvider
    {
        [Inject] private readonly Transform transform;

        public Vector3 Right => transform.right;
        public Vector3 Forward => transform.forward;
    }
}