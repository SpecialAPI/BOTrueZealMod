using MUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.CustomEffects
{
    public class ResurrectInRandomSlotEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            var possibleResurrections = stats.GetPossibleResurrectionCharacters();
            if (possibleResurrections.Count == 0)
                return false;

            var validTargets = new List<TargetSlotInfo>();
            foreach(var t in targets)
            {
                if(t != null && !t.HasUnit && t.IsTargetCharacterSlot)
                    validTargets.Add(t);
            }

            if(validTargets.Count == 0)
                return false;

            var randomTarget = validTargets.GetRandomElement();
            var randomChar = possibleResurrections.GetRandomElement();

            if (stats.ResurrectDeadCharacter(randomChar, randomTarget.SlotID, entryVariable))
                exitAmount++;

            return exitAmount > 0;
        }
    }
}
