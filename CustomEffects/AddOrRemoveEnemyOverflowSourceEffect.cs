using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.CustomEffects
{
    public class AddOrRemoveEnemyOverflowSourceEffect : EffectSO
    {
        public bool add = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (add)
                CombatManager.Instance.GetOrAddComponent<EnemyOverflowHandler>().enemyOverflowSources++;
            else
                CombatManager.Instance.GetOrAddComponent<EnemyOverflowHandler>().enemyOverflowSources--;

            return true;
        }
    }
}
