using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    [HarmonyPatch]
    public static class ComatDialogueNodeVisitedPatch
    {
        [HarmonyPatch(typeof(CombatDialogueHandler), nameof(CombatDialogueHandler.Awake))]
        [HarmonyPostfix]
        private static void AddHandler(CombatDialogueHandler __instance)
        {
            var runner = __instance._combatDialogueRunner;
            var listener = runner.GetOrAddComponent<EvilNodeCompleteListener>();

            runner.onNodeComplete.AddListener(listener.NodeComplete);
        }

        private class EvilNodeCompleteListener : MonoBehaviour
        {
            public IInGameRunData dialogueData;

            public void Awake()
            {
                if (infoHolder == null)
                    return;

                if (!infoHolder.HasRunData)
                    return;

                var runData = infoHolder.Run;
                if (runData.inGameData is not RunInGameData gameRun)
                    return;

                dialogueData = gameRun;
            }

            public void NodeComplete(string nodeName)
            {
                dialogueData?.AddVisitedNode(nodeName);
            }
        }
    }
}
