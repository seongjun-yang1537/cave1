using UnityEngine;
using VContainer;

namespace Ingame
{
    public class PlayerDirectionProvider : IDirectionProvider
    {
        [Inject] private readonly Camera camera;

        public Vector3 Right => camera.transform.right;
        public Vector3 Forward => camera.transform.forward;
    }
}