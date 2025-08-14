using Core;
using UnityEditor;

namespace Domain
{
    [CustomEditor(typeof(GameScope))]
    public class EditorGameScope : EditorScopeBase<GameScope, GameModel, GameModelData, GameModelState>
    {
        protected override GameModel GetModel(GameScope scope) => scope.gameModel;
        protected override GameModelData GetModelData(GameScope scope) => scope.gameModelData;
        protected override GameModelState GetModelState(GameScope scope) => scope.gameModelState;
    }
}