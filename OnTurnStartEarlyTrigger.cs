using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    [HarmonyPatch]
    public static class OnTurnStartEarlyTrigger
    {
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.PlayerTurnStart))]
        [HarmonyILManipulator]
        private static void Characters(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt<CombatManager>($"get_{nameof(CombatManager.Instance)}"), 2))
                return;

            crs.Emit(OpCodes.Ldarg_0);
            crs.EmitStaticDelegate(CallTrigger);
        }

        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.StartTurn))]
        [HarmonyILManipulator]
        public static void Enemies(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchCallOrCallvirt<CombatManager>($"get_{nameof(CombatManager.Instance)}"), 3))
                return;

            crs.Emit(OpCodes.Ldarg_0);
            crs.EmitStaticDelegate(CallTrigger);
        }

        private static CombatManager CallTrigger(CombatManager curr, IUnit unit)
        {
            CombatManager.Instance.PostNotification(TriggerCallsE.OnTurnStart_Early.ToString(), unit, null);

            return curr;
        }
    }
}
