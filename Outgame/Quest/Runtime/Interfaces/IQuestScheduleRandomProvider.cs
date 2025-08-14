using System.Collections.Generic;
using Corelib.Utils;

namespace Quest
{
    public interface IQuestScheduleRandomProvider
    {
        List<QuestGenerationConfig> Generate(int day, MT19937 rng);
    }
}
