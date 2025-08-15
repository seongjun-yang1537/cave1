namespace UI
{
    public abstract class TooltipUIModel
    {
        public readonly UIMonoBehaviour bindUI;
        public int priority = 0;

        public TooltipUIModel(UIMonoBehaviour bindUI)
        {
            this.bindUI = bindUI;
        }
    }
}