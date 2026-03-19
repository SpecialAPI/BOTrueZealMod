using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.CustomEffects
{
    public class SwapTowardsCasterEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            var characters = new List<IUnit>();
            var enemies = new List<IUnit>();
            for (int i = 0; i < targets.Length; i++)
            {
                if (!targets[i].HasUnit)
                    continue;

                var u = targets[i].Unit;
                if (u.IsUnitCharacter && !characters.Contains(u))
                    characters.Add(u);
                else if (!u.IsUnitCharacter && !enemies.Contains(u))
                    enemies.Add(u);
            }

            var leftmostCasterSlot = caster.SlotID;
            var rightmostCasterSlot = caster.SlotID + caster.Size - 1;

            foreach (IUnit ch in characters)
            {
                var dist1 = ch.SlotID - leftmostCasterSlot;
                var dist2 = ch.SlotID + ch.Size - 1 - rightmostCasterSlot;
                var totalDist = dist1 + dist2;

                if (totalDist == 0)
                    continue;

                var moveDir = (totalDist >= 0) ? -1 : 1;
                if (ch.SlotID + moveDir < 0 || ch.SlotID + moveDir >= stats.combatSlots.CharacterSlots.Length)
                    continue;

                if (stats.combatSlots.SwapCharacters(ch.SlotID, ch.SlotID + moveDir, true))
                    exitAmount++;
            }

            foreach (IUnit en in enemies)
            {
                var dist1 = en.SlotID - leftmostCasterSlot;
                var dist2 = en.SlotID + en.Size - 1 - rightmostCasterSlot;
                var totalDist = dist1 + dist2;

                if (totalDist == 0)
                    continue;

                var moveDir = (totalDist >= 0) ? -1 : en.Size;
                if (!stats.combatSlots.CanEnemiesSwap(en.SlotID, en.SlotID + moveDir, out var firstSlotSwap, out var secondSlotSwap))
                    continue;

                if (stats.combatSlots.SwapEnemies(en.SlotID, firstSlotSwap, en.SlotID + moveDir, secondSlotSwap, isMandatory: true))
                {
                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }
}
