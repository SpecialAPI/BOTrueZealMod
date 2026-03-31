using BOTrueZealMod.CustomEffects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Characters
{
    public static class ShellyK
    {
        private static GameObject FreeFoolRoom;
        
        public static void Init()
        {
            var ch = NewCharacter("ShellyK_CH", EntityIDsE.ShellyK);
            ch.SetBasicInformation("Shelly K.", Pigments.Purple, "ShellyFront", "ShellyBack", "ShellyOW");
            ch.SetSounds("event:/TrueZeal/ShellyHurt", "event:/TrueZeal/ShellyDeath", "event:/TrueZeal/ShellyDX");

            ch.RankedDataSetup(4, (rank, abRank) =>
            {
                var relapseDmgHeal = RankedValue(4, 6, 8, 10);
                var relapseName = $"{RankedValue("Gradual", "Accelerating", "Sudden", "Total")} Relapse";
                var relapseDesc = $"Heal {relapseDmgHeal} health to any party members or enemies adjacent to this party member with health lower than this party member.\nDeal {relapseDmgHeal} damage to any party members or enemies adjacent to this party member with health higher than this party member.";
                var relapse = AbilityBuilder.NewAbility($"Relapse_{abRank}_A")
                .SetBasicInformationCharacter(relapseName, relapseDesc, "AttackIcon_Relapse")
                .SetEffects(new()
                {
                    Effects.Effect(Targets.Relative(true, -1, 1), CreateScriptable<RelapseEffect>(), relapseDmgHeal),
                    Effects.Effect(Targets.Relative(false, -1, 0, 1), CreateScriptable<RelapseEffect>(), relapseDmgHeal)
                })
                .SetIntents(new()
                {
                    TargetIntent(Targets.Relative(false, -1, 0, 1), IntentForDamage(relapseDmgHeal), IntentForHealing(relapseDmgHeal)),
                    TargetIntent(Targets.Relative(true, -1, 1), IntentForDamage(relapseDmgHeal), IntentForHealing(relapseDmgHeal))
                })
                .SetVisuals(CustomAnimations.Relapse, Targets.Self)
                .CharacterAbility(Pigments.Red, Pigments.Blue, Pigments.Red);

                var flirtDmg = RankedValue(6, 8, 10, 12);
                var flirtHeal = RankedValue(3, 4, 5, 6);
                var flirtName = $"{RankedValue("Playful", "Tempting", "Seductive", "Irresistible")} Flirtation";
                var flirtDesc = $"Deal {flirtDmg} indirect damage to the enemies that are the farthest away from this party member.\nHeal the enemies damaged by this ability {flirtHeal} health and pull them closer to this party member.";
                var flirt = AbilityBuilder.NewAbility($"Flirtation_{abRank}_A")
                .SetBasicInformationCharacter(flirtName, flirtDesc, "AttackIcon_Flirtation")
                .SetEffects(new()
                {
                    Effects.Effect(Targets.FurthestOpponents, CreateScriptable<DamageEffect>(x => x._indirect = true), flirtDmg),
                    Effects.Effect(Targets.FurthestOpponents, CreateScriptable<HealEffect>(x => x._onlyIfHasHealthOver0 = true), flirtHeal),
                    Effects.Effect(Targets.FurthestOpponents, CreateScriptable<SwapTowardsCasterEffect>()),
                })
                .SetIntents(new()
                {
                    TargetIntent(Targets.FurthestOpponents, IntentForDamage(flirtDmg), IntentForHealing(flirtHeal), IntentType.Swap_Sides)
                })
                .SetVisuals(CustomAnimations.Flirt, Targets.FurthestOpponents)
                .CharacterAbility(Pigments.Red, Pigments.Purple, Pigments.Red);

                var brawlRupture = RankedValue(2, 3, 3, 4);
                var brawlOil = RankedValue(2, 3, 3, 4);
                var brawlOilTargets = RankedValue(Targets.Front, Targets.Front, Targets.Relative(false, -1, 0, 1), Targets.Relative(false, -1, 0, 1));
                var brawlOilEnemyText = RankedValue("Opposing enemy", "Opposing enemy", "Opposing, Left and Right enemies", "Opposing, Left and Right enemies");
                var brawlDamage = RankedValue(2, 3, 4, 5);
                var brawlName = $"{RankedValue("Drunken", "Pub", "Bar", "Infamous")} Brawl";
                var brawlDesc = $"Inflict {brawlRupture} Ruptured to the enemies that are the farthest away from this party member.\nInflict {brawlOil} Oil-Slicked to the {brawlOilEnemyText}.\nDeal {brawlDamage} damage to enemies currently inflicted with Oil-Slicked.";
                var brawl = AbilityBuilder.NewAbility($"Brawl_{abRank}_A")
                .SetBasicInformationCharacter(brawlName, brawlDesc, "AttackIcon_Brawl")
                .SetEffects(new()
                {
                    Effects.Effect(Targets.FurthestOpponents, CreateScriptable<ApplyRupturedEffect>(), brawlRupture),
                    Effects.Effect(brawlOilTargets, CreateScriptable<ApplyOilSlickedEffect>(), brawlOil),
                    Effects.Effect(Targets.UnitsWithStatus(false, StatusEffectType.OilSlicked), CreateScriptable<DamageEffect>(), brawlDamage)
                })
                .SetIntents(new()
                {
                    TargetIntent(Targets.FurthestOpponents, IntentType.Status_Ruptured),
                    TargetIntent(brawlOilTargets, IntentType.Status_OilSlicked),
                    TargetIntent(Targets.UnitsWithStatus(false, StatusEffectType.OilSlicked), IntentForDamage(brawlDamage))
                })
                .SetVisuals(CustomAnimations.BarFight, Targets.FurthestOpponents)
                .CharacterAbility(Pigments.Red, Pigments.Yellow, Pigments.Red);

                return new()
                {
                    health = RankedValue(17, 19, 21, 24),
                    rankAbilities = [relapse, flirt, brawl]
                };
            });
            ch.AddToDatabase(true, true);

            var questAch = AchievementBuilder.NewAchievement(AchievementIDs.ShellyQuest, "The Cougar", "Unlock Shelly K. for real this time...")
                .SetSprite("Shelly_Quest")
                .AddToBaseCategory(AchievementUnlockType.PartyMembers);
            var questUnlock = UnlockBuilder.NewUnlock(UnlockableIDE.ShellyK)
                .SetAchievement(questAch)
                .SetCharacter(GetID("ShellyK_CH"))
                .SetQuest(QuestIDsE.ShellyKQuest)
                .AddToDatabase();

            var osmanAch = AchievementBuilder.NewAchievement(AchievementIDs.ShellyOsmanUnlock, "Burn-Bottle Batch", "Unlocked a new item.")
                .SetSprite("Shelly_Osman")
                .AddToBaseCategory(AchievementUnlockType.TheWitness);
            var osmanUnlock = UnlockBuilder.NewUnlock(UnlockableIDE.ShellyK_Osman)
                .SetAchievement(osmanAch)
                .SetItems([GetID("BurnBottleBatch_SW")]);
            ch.AddFinalBossUnlock(BossType.OsmanSinnoks, osmanUnlock);

            var heavenAch = AchievementBuilder.NewAchievement(AchievementIDs.ShellyHeavenUnlock, "Royal Pine", "Unlocked a new item.")
                .SetSprite("Shelly_Heaven")
                .AddToBaseCategory(AchievementUnlockType.TheDivine);
            var heavenUnlock = UnlockBuilder.NewUnlock(UnlockableIDE.ShellyK_Heaven)
                .SetAchievement(heavenAch)
                .SetItems([GetID("RoyalPine_TW")]);
            ch.AddFinalBossUnlock(BossType.Heaven, heavenUnlock);

            var unlockTracker = CreateScriptable<CustomGameBoolTrackData>();
            unlockTracker.boolDataKey = "TrueZeal_ShellyKFirstBarTalkDone";
            unlockTracker.locID = "TrueZeal_TrackerCharShellyK";
            unlockTracker.defaultText = "This party member needs to be seduced by a real man.";

            var menuCh = ch.GenerateMenuCharacter("ShellyUnlocked", "ShellyLocked");
            menuCh.SetAsFullDPS();
            menuCh.AddToDatabase();
            menuCh.SetOsmanAchievement(AchievementIDs.ShellyOsmanUnlock);
            menuCh.SetHeavenAchievement(AchievementIDs.ShellyHeavenUnlock);
            menuCh._trackData = unlockTracker;

            var speaker = CreateScriptable<SpeakerData>();
            speaker.name = GetID("ShellyK_SpeakerData");
            speaker.speakerName = "Shelly K.";
            speaker.portraitLooksCenter = false;
            speaker.portraitLooksLeft = true;
            speaker._emotionBundles =
            [
                new()
                {
                    emotion = "Drink",
                    bundle = new()
                    {
                        portrait = LoadSprite("ShellyDrink"),
                        dialogueSound = ch.dxSound,
                        bundleTextColor = new(0.6078f, 0.6784f, 0.7176f)
                    }
                },
                new()
                {
                    emotion = "Smirk",
                    bundle = new()
                    {
                        portrait = LoadSprite("ShellySmirk"),
                        dialogueSound = ch.dxSound,
                        bundleTextColor = new(0.6078f, 0.6784f, 0.7176f)
                    }
                },
                new()
                {
                    emotion = "Blush",
                    bundle = new()
                    {
                        portrait = LoadSprite("ShellyBlush"),
                        dialogueSound = ch.dxSound,
                        bundleTextColor = new(0.7176f, 0.5088f, 0.5088f)
                    }
                },
            ];
            speaker._defaultBundle = new()
            {
                portrait = ch.characterSprite,
                dialogueSound = ch.dxSound,
                bundleTextColor = new(0.6078f, 0.6784f, 0.7176f)
            };
            LoadedAssetsHandler.LoadedSpeakers[speaker.name] = speaker;

            var room = Bundle.LoadAsset<GameObject>("TrueZeal_FreeFool_ShellyK_ER");
            var roomHandler = room.AddComponent<NPCRoomHandler>();
            roomHandler._requiresToTalk = false;
            roomHandler._dialogueMusic = string.Empty;
            LoadedAssetsHandler.LoadedRoomPrefabs[$"Encounters/{roomHandler.name}"] = roomHandler;
            FreeFoolRoom = room;

            var npc = room.transform.Find("NPCRoomItemSelectable_Template").gameObject;
            var npcItem = npc.AddComponent<BasicRoomItem>();
            npcItem._renderers = npc.GetComponentsInChildren<SpriteRenderer>();
            npcItem._detector = npc.GetComponent<BoxCollider2D>();
            roomHandler._npcSelectable = npcItem;
            var npcOutlineMat = (LoadedAssetsHandler.GetRoomPrefab(CardType.Flavour, "Flavour_PervertMessiah_ER") as NPCRoomHandler)._npcSelectable._renderers[0].material;
            foreach (var s in npcItem._renderers)
            {
                if (s == null)
                    continue;

                s.material = new Material(npcOutlineMat);
                s.material.SetFloat("_OutlineAlpha", 0f);
            }

            PortalSignAdder.AddSign(SignTypeE.ShellyK, LoadSprite("ShellyOW", new(0.5f, 0f)));
            var freefool = CreateScriptable<FreeFoolEncounterSO>();
            freefool._freeFool = ch.name;
            freefool._dialogue = Dialogues.ShellyFreeFool.name;
            freefool.encounterRoom = roomHandler.name;
            freefool.npcEntityIDs = [ch.characterEntityID];
            freefool.signType = SignTypeE.ShellyK;
            freefool.name = GetID("ShellyK_FreeFoolEncounter");
            LoadedAssetsHandler.LoadedFreeFoolEncounters[freefool.name] = freefool;
            Zones.Hard1._FreeFoolsPool = Zones.Hard1._FreeFoolsPool.AddToArray(freefool.name);
        }
    }
}
