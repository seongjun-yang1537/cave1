using Corelib.Utils;

namespace Domain
{
    public class GamePlayerController : ILifecycleInjectable
    {
        private readonly GameController controller;
        private GameModel gameModel => controller.gameModel;

        public GamePlayerController(GameController controller)
        {
            this.controller = controller;
        }
    }
}