using UnityEngine;

namespace Ingame
{
    public class JetpackController : MonoBehaviour
    {
        public VFXJetpack vfxJetpack;

        public void OnEnableJetpack()
        {
            vfxJetpack.Render(JetpackState.On);
        }

        public void OnDisableJetpack()
        {
            vfxJetpack.Render(JetpackState.Off);
        }
    }
}