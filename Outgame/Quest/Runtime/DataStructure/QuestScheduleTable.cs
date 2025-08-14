using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Corelib.Utils;
using Core;

namespace Quest
{
    [CreateAssetMenu(menuName = "Game/Quest/Schedule Table")]
    public class QuestScheduleTable : ScriptableObject
    {
        [System.Serializable]
        public class Entry
        {
            public IntRange days;
            [SerializeField]
            public List<QuestScheduleData> schedules;
        }
        [SerializeField]
        public List<Entry> table;

        public List<QuestModel> Generate(int day)
        {
            return table
                .Where(e => e.days.Contains(day))
                .SelectMany(e => e.schedules)
                .SelectMany(schedule => schedule.Generate(day, GameRng.Game))
                .ToList();
        }

        public List<QuestScheduleData> Get(int day)
        {
            var entry = table.FirstOrDefault(e => e.days.Contains(day));
            if (entry == null) return new List<QuestScheduleData>();
            return entry.schedules;
        }

        public void Sort()
        {
            table.Sort((a, b) => a.days.Min.CompareTo(b.days.Min));
        }
    }
}
