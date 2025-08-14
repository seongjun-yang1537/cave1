using Core;
using Corelib.Utils;
using TriInspector;
using UnityEngine;

namespace Ingame
{
    public class EntityServiceLocator : Singleton<EntityServiceLocator>
    {
        [SerializeReference]
        private IEntityWorldHandler worldHandler;
        public static IEntityWorldHandler WorldHandler => Instance.worldHandler;

        [SerializeReference]
        private IEntityUIHandler uiHandler;
        public static IEntityUIHandler UIHandler => Instance.uiHandler;

        [SerializeReference]
        private IEntityGameHandler gameHandler;
        public static IEntityGameHandler GameHandler => Instance.gameHandler;
    }
}