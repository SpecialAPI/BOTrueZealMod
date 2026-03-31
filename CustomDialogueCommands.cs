using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    [HarmonyPatch]
    public static class CustomDialogueCommands
    {
        public const string UnlockCombat = "TrueZeal_UnlockCombat";
        public const string HireCharacterCombat = "TrueZeal_HireCharacterCombat";
        public const string SaveProgressCombat = "TrueZeal_SaveProgressCombat";

        public const string HasPartySpaceCombat = "TrueZeal_HasPartySpaceCombat";
        public const string VisitedCombat = "TrueZeal_VisitedCombat";

        [HarmonyPatch(typeof(CombatDialogueHandler), nameof(CombatDialogueHandler.Awake))]
        [HarmonyPostfix]
        private static void AddCombatDialogueCommands(CombatDialogueHandler __instance)
        {
            var runner = __instance._combatDialogueRunner;

            runner.AddCommandHandler(UnlockCombat, x =>
            {
                if (!Enum.TryParse<UnlockableID>(x[0], out var result))
                    return;

                infoHolder.UnlockableManager.TryUnlockContentFromID(result);
            });
            runner.AddCommandHandler(HireCharacterCombat, x =>
            {
                if (infoHolder == null)
                    return;

                if (!infoHolder.HasRunData)
                    return;

                var runData = infoHolder.Run;
                runData.TryHireCharacter(x);
            });
            runner.AddCommandHandler(SaveProgressCombat, x =>
            {
                CombatManager.Instance.FullSaveGame();
            });

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
            runner.AddFunction(VisitedCombat, 1, parameters =>
            {
                if (infoHolder == null)
                    return false;

                if (!infoHolder.HasRunData)
                    return false;

                var runData = infoHolder.Run;
                if(runData.inGameData is not RunInGameData gameRun)
                    return false;

                var value = parameters[0];
                return gameRun.ContainsVisitedNode(value.AsString);
            });
        }
    }
}
