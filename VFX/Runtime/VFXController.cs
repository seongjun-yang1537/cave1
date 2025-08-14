using UnityEngine;
namespace VFX
{
    public class VFXController : MonoBehaviour
    {
        public float duration = float.MaxValue;
        public void SetDuration(float duration = float.MaxValue)
        {
            this.duration = duration;
        }

        public void Update()
        {
            if (duration <= 0f) Destroy(gameObject);
            duration -= Time.deltaTime;
        }
    }
}