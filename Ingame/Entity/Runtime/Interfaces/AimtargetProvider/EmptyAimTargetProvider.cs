namespace Ingame
{
    public class EmptyAimTargetProvider : IAimTargetProvider
    {
        public int GetAimtarget()
        {
            return -1;
        }
    }
}