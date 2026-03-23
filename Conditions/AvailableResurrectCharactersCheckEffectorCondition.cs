using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Conditions
{
    public class AvailableResurrectCharactersCheckEffectorCondition : EffectorConditionSO
    {
        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            var stats = CombatManager.Instance._stats;

            return stats.GetPossibleResurrectionCharacters().Count > 0;
        }
    }
}
