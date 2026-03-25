using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Conditions
{
    public class HasCustomAchievementEffectCondition : EffectConditionSO
    {
        public string achID;
        public bool needsToBeUnlocked = true;

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return AchievementBuilder.IsCustomAchievementUnlocked(achID) == needsToBeUnlocked;
        }
    }
}
