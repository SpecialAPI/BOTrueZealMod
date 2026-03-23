using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    [HarmonyPatch]
    public class EnemyOverflowHandler : MonoBehaviour
    {
        public int enemyOverflowSources;

        [HarmonyPatch(typeof(PlayerTurnEndSecondPartAction), nameof(PlayerTurnEndSecondPartAction.CalculateOverflow))]
        [HarmonyILManipulator]
        private static void MaybeDoEnemyOverflowTranspiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchStloc(2)))
                return;

            var enemyOverflowPercentLocal = crs.DeclareLocal<int>();

            crs.EmitStaticDelegate(Zero);
            crs.Emit(OpCodes.Stloc, enemyOverflowPercentLocal);

            if(!crs.JumpToNext(x => x.MatchStloc(2)))
                return;

            crs.Emit(OpCodes.Ldloc_2);
            crs.Emit(OpCodes.Stloc, enemyOverflowPercentLocal);

            if(!crs.JumpBeforeNext(x => x.OpCode == OpCodes.Leave || x.OpCode == OpCodes.Leave_S))
                return;

            crs.Emit(OpCodes.Ldarg_1);
            crs.Emit(OpCodes.Ldloc, enemyOverflowPercentLocal);
            crs.EmitStaticDelegate(DealEnemyOverflowDamage);
        }

        private static int Zero() => 0;

        private static void DealEnemyOverflowDamage(CombatStats stats, int enemyOverflowPercent)
        {
            if (CombatManager.Instance.GetOrAddComponent<EnemyOverflowHandler>().enemyOverflowSources <= 0 || enemyOverflowPercent <= 0)
                return;

            foreach(var en in stats.EnemiesOnField.Values)
            {
                if (!en.IsAlive)
                    continue;

                var dmgAmt = en.CalculatePercentualAmount(enemyOverflowPercent);
                en.ManaDamage(dmgAmt, true, DeathType.Overflow);
            }
        }
    }
}
