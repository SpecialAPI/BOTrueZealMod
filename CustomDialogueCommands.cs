using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    [HarmonyPatch]
    public static class CustomDialogueCommands
    {
        public const string HasPartySpaceCombat = "TrueZeal_HasPartySpaceCombat";

        [HarmonyPatch(typeof(CombatDialogueHandler), nameof(CombatDialogueHandler.Awake))]
        [HarmonyPostfix]
        private static void AddCombatDialogueCommands(CombatDialogueHandler __instance)
        {
            var runner = __instance._combatDialogueRunner;

            runner.AddFunction(HasPartySpaceCombat, 0, parameters =>
            {
                if (infoHolder == null)
                    return false;

                if (!infoHolder.HasRunData)
                    return false;

                var runData = infoHolder.Run;
                if (runData.PlayerDialogueData is not IPlayerDialogueData playerData)
                    return false;

                return playerData.HasCharacterSpace;
            });
        }
    }
}
