using UnityEngine;

namespace Ingame
{
    public interface IDirectionProvider
    {
        public Vector3 Right { get; }
        public Vector3 Forward { get; }
    }
}