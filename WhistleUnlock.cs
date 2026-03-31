using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    [HarmonyPatch]
    public static class WhistleUnlock
    {
        public static void Init()
        {
            var ach = AchievementBuilder.NewAchievement(AchievementIDs.ShellyTragedy, "Plenty of Fish in the Desert", "Watch Anton's desires go down in flames... </3")
                .SetSprite("Shelly_Tragedy")
                .AddToBaseCategory(AchievementUnlockType.Tragedies);
            UnlockBuilder.NewUnlock(UnlockableIDE.AntonSad)
                .SetAchievement(ach)
                .SetItems([GetID("WailingWhistle_SW")])
                .AddToDatabase();
        }

        [HarmonyPatch(typeof(CombatManager), nameof(CombatManager.InitializeCombat))]
        [HarmonyILManipulator]
        private static void SetUpNotifsTranspiler(ILContext ctx)
        {
            var crs = new ILCursor(ctx);

            if (!crs.JumpToNext(x => x.MatchNewobj<CombatStartAction>()))
                return;

            crs.EmitStaticDelegate(SetUpNotifications);
        }

        private static CombatStartAction SetUpNotifications(CombatStartAction curr)
        {
            CombatManager.Instance.AddObserver(CheckUnlock, TriggerCalls.OnDeath.ToString());

            return curr;
        }

        public static void CheckUnlock(object sender, object args)
        {
            if(AchievementBuilder.IsCustomAchievementUnlocked(AchievementIDs.ShellyTragedy))
                return;

            if(sender is not IUnit killedUnit)
                return;

            if (args is not DeathReference deathRef || deathRef.killer is not IUnit killer)
                return;

            if (killedUnit.IsUnitCharacter || !killer.IsUnitCharacter || killer.GetEntityID() != EntityIDs.Anton)
                return;

            var hasShelly = false;
            foreach(var cc in CombatManager.Instance._stats.CharactersOnField.Values)
            {
                if (cc.GetEntityID() != EntityIDsE.ShellyK)
                    continue;

                hasShelly = true;
                break;
            }

            if (!hasShelly)
                return;

            var shellyDialogue = Dialogues.ShellyCombat;
            CombatManager.Instance.AddUIAction(new PlayDialogueUIAction(shellyDialogue.m_DialogID, shellyDialogue.startNode, shellyDialogue.dialog));
        }
    }
}
