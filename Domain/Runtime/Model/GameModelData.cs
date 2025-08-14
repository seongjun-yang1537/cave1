using System.Collections.Generic;
using GameTime;
using Quest;
using UnityEngine;

namespace Domain
{
    [CreateAssetMenu(fileName = "GameModelData", menuName = "Game/Game Model Data")]
    public class GameModelData : ScriptableObject
    {
        [SerializeField]
        public GamePlayerModelData gamePlayer;

        [SerializeField]
        public GameCalendarModelData calendar;
    }
}