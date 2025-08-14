namespace Ingame
{
    public interface IJetpackable
    {
        public bool CanJetpack();
        public void ActivateJetpack();
        public void ProcessJetpack(float delta);
        public void DeactivateJetpack();
    }
}