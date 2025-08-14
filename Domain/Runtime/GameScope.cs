using Quest;
using VContainer;
using VContainer.Unity;

namespace Domain
{
    public class GameScope : LifetimeScope
    {
        #region ========== Input ==========
        public GameModelData gameModelData;
        public GameModelState gameModelState;
        #endregion ====================

        #region ========== Runtime ==========
        public GameModel gameModel;
        #endregion ====================

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(gameModel = GameModel.FromState(gameModelState))
                   .As<GameModel>()
                   .AsSelf();

            builder.RegisterComponent(GetComponent<GameController>())
                   .AsSelf()
                   .AsImplementedInterfaces();

            builder.Register<PlayerQuestProvider>(Lifetime.Singleton)
               .As<IPlayerQuestProvider>();
        }
    }
}
