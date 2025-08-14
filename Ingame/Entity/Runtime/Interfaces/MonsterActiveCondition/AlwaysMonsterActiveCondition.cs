using UnityEngine;
using VContainer;

namespace Ingame
{
    public class AlwaysMonsterActiveCondition : IMonsterActiveCondition
    {
        public bool IsActive()
        {
            return true;
        }
    }
}
