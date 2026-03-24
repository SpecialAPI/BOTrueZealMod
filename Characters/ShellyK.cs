using BOTrueZealMod.CustomEffects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Characters
{
    public static class ShellyK
    {
        public static void Init()
        {
            var ch = NewCharacter("ShellyK_CH", EntityIDsE.ShellyK);
            ch.SetBasicInformation("Shelly K.", Pigments.Purple, "ShellyFront", "ShellyBack", "ShellyOW");

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
                .SetVisuals(Animations.OiledBurn, Targets.Self)
                .CharacterAbility(Pigments.Red, Pigments.Blue, Pigments.Red);

                var flirtDmg = RankedValue(6, 8, 10, 12);
                var flirtHeal = RankedValue(3, 4, 5, 6);
                var flirtName = $"{RankedValue("Playful", "Tempting", "Seductive", "Irresistible")} Flirtation";
                var flirtDesc = $"Deal {flirtDmg} indirect damage to the enemies that are the farthest away from this party member.\nHeal the enemies damaged by this ability {flirtHeal} health and pull them closer to this party member.";
                var flirt = AbilityBuilder.NewAbility($"Flirtation_{abRank}_A")
                .SetBasicInformationCharacter(flirtName, flirtDesc, "AttackIcon_Flirtation")
                .SetEffects(new()
                {
                    Effects.Effect(Targets.FurthestOpponents, CreateScriptable<DamageEffect>(), flirtDmg),
                    Effects.Effect(Targets.FurthestOpponents, CreateScriptable<HealEffect>(x => x._onlyIfHasHealthOver0 = true), flirtHeal),
                    Effects.Effect(Targets.FurthestOpponents, CreateScriptable<SwapTowardsCasterEffect>()),
                })
                .SetIntents(new()
                {
                    TargetIntent(Targets.FurthestOpponents, IntentForDamage(flirtDmg), IntentForHealing(flirtHeal), IntentType.Swap_Sides)
                })
                .SetVisuals(Animations.Kiss, Targets.FurthestOpponents)
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
                .SetVisuals(Animations.EatMyFlesh, Targets.FurthestOpponents)
                .CharacterAbility(Pigments.Red, Pigments.Yellow, Pigments.Red);

                return new()
                {
                    health = RankedValue(17, 19, 21, 24),
                    rankAbilities = [relapse, flirt, brawl]
                };
            });
            ch.AddToDatabase(true, false);

            var osmanAch = AchievementBuilder.NewAchievement(AchievementIDs.ShellyOsmanUnlock, "Burn-Bottle Batch", "Unlocked a new item.")
                .SetSprite("Shelly_Osman")
                .AddToBaseCategory(AchievementUnlockType.TheWitness);
            var osmanUnlock = UnlockBuilder.NewUnlock(UnlockableIDE.ShellyK_Osman)
                .SetAchievement(osmanAch)
                .SetItems(["BurnBottleBatch_SW"])
                .AddToDatabase();
            ch.AddFinalBossUnlock(BossType.OsmanSinnoks, osmanUnlock);

            var heavenAch = AchievementBuilder.NewAchievement(AchievementIDs.ShellyHeavenUnlock, "Royal Pine", "Unlocked a new item.")
                .SetSprite("Shelly_Heaven")
                .AddToBaseCategory(AchievementUnlockType.TheDivine);
            var heavenUnlock = UnlockBuilder.NewUnlock(UnlockableIDE.ShellyK_Heaven)
                .SetAchievement(heavenAch)
                .SetItems(["RoyalPine_TW"])
                .AddToDatabase();
            ch.AddFinalBossUnlock(BossType.Heaven, heavenUnlock);

            var menuCh = ch.GenerateMenuCharacter("ShellyUnlocked", "ShellyLocked");
            menuCh.SetAsFullDPS();
            menuCh.AddToDatabase();
        }
    }
}
