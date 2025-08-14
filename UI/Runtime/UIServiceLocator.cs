using Corelib.Utils;
using UnityEngine;

namespace UI
{
    public class UIServiceLocator : Singleton<UIServiceLocator>
    {
        [SerializeReference]
        private IUIGameHandler gameHandler;
        public static IUIGameHandler GameHandler => Instance.gameHandler;
    }
}