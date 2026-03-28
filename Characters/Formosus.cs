using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Characters
{
    public static class Formosus
    {
        public static void Init()
        {
            var ch = NewCharacter("Formosus_CH", EntityIDsE.Formosus);
            ch.SetBasicInformation("Formosus", Pigments.Purple, "FormosusFront", "FormosusBack", "FormosusOW");
            ch.SetSounds("event:/TrueZeal/FormosusHurt", "event:/TrueZeal/FormosusDeath", "event:/TrueZeal/FormosusDX");
            ch.AddPassives(CustomPassives.Anointed(1));

            ch.RankedDataSetup(4, (rank, abRank) =>
            {
                var massMinDamage = RankedValue(1, 2, 2, 3);
                var massMaxDamage = RankedValue(2, 2, 3, 4);
                var massDmgIsRng = massMinDamage != massMaxDamage;
                var massDmgRangeString = massDmgIsRng ? $"{massMinDamage}-{massMaxDamage}" : massMinDamage.ToString();
                var massName = $"{RankedValue("Hold", "Sacred", "Midnight", "Eternal")} Mass";
                var massDesc = $"Deal {massDmgRangeString} indirect damage to All enemies with status effects applied to them.\nHeal All party members with status effects an equivalent amount of health to damage dealt.";
                var mass = AbilityBuilder.NewAbility($"Mass_{abRank}_A")
                .SetBasicInformationCharacter(massName, massDesc, "AttackIcon_Mass")
                .SetEffects([
                    ..massDmgIsRng ? new List<EffectInfo>()
                    {
                        Effects.Effect(null, CreateScriptable<ExtraVariableForNextEffect>(), massMinDamage),
                        Effects.Effect(Targets.OpponentsWithStatuses, CreateScriptable<RandomDamageBetweenPreviousAndEntryEffect>(x => x._indirect = true), massMaxDamage)
                    } :
                    [Effects.Effect(Targets.OpponentsWithStatuses, CreateScriptable<DamageEffect>(x => x._indirect = true), massMinDamage)],

                    Effects.Effect(Targets.AlliesWithStatuses, CreateScriptable<HealEffect>(x => x.usePreviousExitValue = true), 1)
                ])
                .SetIntents(new()
                {
                    TargetIntent(Targets.OpponentsWithStatuses, IntentForDamage(massMaxDamage)),
                    TargetIntent(Targets.AlliesWithStatuses, IntentForHealing(massMaxDamage * 5))
                })
                .SetVisuals(CustomAnimations.Bell, Targets.OpponentsWithStatuses)
                .CharacterAbility(Pigments.Purple, RankedValue(Pigments.Yellow, Pigments.SplitPigment(Pigments.Yellow, Pigments.Purple), Pigments.SplitPigment(Pigments.Yellow, Pigments.Purple), Pigments.SplitPigment(Pigments.Yellow, Pigments.Purple)));

                var confessionStatusIncrease = RankedValue(1, 2, 2, 3);
                var confessionDivineProtection = 1;
                var confessionName = $"{RankedValue("False", "Sinful", "Damning", "True")} Confession";
                var confessionDesc = $"Increase the duration of all status effects inflicted to All enemies by {confessionStatusIncrease}.\nInflict {confessionDivineProtection} Divine Protection to the opposing enemy.";
                var confession = AbilityBuilder.NewAbility($"Confession_{abRank}_A")
                .SetBasicInformation(confessionName, confessionDesc, "AttackIcon_Confession")
                .SetEffects(new()
                {
                    Effects.Effect(Targets.OpponentsWithStatuses, CreateScriptable<IncreaseOnlyStatusEffectsEffect>(x => x.positive = false), confessionStatusIncrease),
                    Effects.Effect(Targets.OpponentsWithStatuses, CreateScriptable<IncreaseOnlyStatusEffectsEffect>(x => x.positive = true), confessionStatusIncrease),
                    Effects.Effect(Targets.Front, CreateScriptable<ApplyDivineProtectionEffect>(), confessionDivineProtection)
                })
                .SetIntents(new()
                {
                    TargetIntent(Targets.OpponentsWithStatuses, IntentType.Misc),
                    TargetIntent(Targets.Front, IntentType.Status_DivineProtection),
                })
                .SetVisuals(CustomAnimations.Scales, Targets.OpponentsWithStatuses)
                .CharacterAbility(Pigments.Purple, RankedValue(Pigments.Blue, Pigments.Blue, Pigments.SplitPigment(Pigments.Blue, Pigments.Purple), Pigments.SplitPigment(Pigments.Blue, Pigments.Purple)));

                var seeDamage = RankedValue(8, 10, 12, 15);
                var seeTargets = Targets.UnitsWithStatus(false, StatusEffectType.DivineProtection);
                var seeName = $"{RankedValue("Holy", "Roman", "Petrine", "Apostolic")} See";
                var seeDesc = $"Deal {seeDamage} damage to All enemies inflicted with Divine Protection.";
                var see = AbilityBuilder.NewAbility($"See_{abRank}_A")
                .SetBasicInformation(seeName, seeDesc, "AttackIcon_See")
                .SetEffects(new()
                {
                    Effects.Effect(seeTargets, CreateScriptable<DamageEffect>(), seeDamage)
                })
                .SetIntents(new()
                {
                    TargetIntent(seeTargets, IntentForDamage(seeDamage))
                })
                .SetVisuals(CustomAnimations.Providence, seeTargets)
                .CharacterAbility(Pigments.Purple, Pigments.Red);

                return new()
                {
                    rankAbilities = [mass, confession, see],
                    health = RankedValue(11, 14, 17, 19)
                };
            });

            ch.AddToDatabase(true, true);

            var questAch = AchievementBuilder.NewAchievement(AchievementIDs.FormosusQuest, "The Exhumed", "Unlock Formosus.")
                .SetSprite("Formosus_Quest")
                .AddToBaseCategory(AchievementUnlockType.PartyMembers);
            var questUnlock = UnlockBuilder.NewUnlock(UnlockableIDE.Formosus)
                .SetAchievement(questAch)
                .SetCharacter(GetID("Formosus_CH"))
                .SetQuest(QuestIDsE.FormosusQuest)
                .AddToDatabase();

            var osmanAch = AchievementBuilder.NewAchievement(AchievementIDs.FormosusOsmanUnlock, "Coelacanth", "Unlocked a new item.")
                .SetSprite("Formosus_Osman")
                .AddToBaseCategory(AchievementUnlockType.TheWitness);
            var osmanUnlock = UnlockBuilder.NewUnlock(UnlockableIDE.Formosus_Osman)
                .SetAchievement(osmanAch)
                .SetItems([GetID("Coelacanth_ExtraW")]);
            ch.AddFinalBossUnlock(BossType.OsmanSinnoks, osmanUnlock);

            var heavenAch = AchievementBuilder.NewAchievement(AchievementIDs.FormosusHeavenUnlock, "Sacred Shrub", "Unlocked a new item.")
                .SetSprite("Formosus_Heaven")
                .AddToBaseCategory(AchievementUnlockType.TheDivine);
            var heavenUnlock = UnlockBuilder.NewUnlock(UnlockableIDE.Formosus_Heaven)
                .SetAchievement(heavenAch)
                .SetItems([GetID("SacredShrub_TW")]);
            ch.AddFinalBossUnlock(BossType.Heaven, heavenUnlock);

            var menuCh = ch.GenerateMenuCharacter("FormosusUnlocked", "FormosusLocked");
            menuCh.AddDPSSets(0, 1);
            menuCh.AddSupportSets(2);
            menuCh.AddToDatabase();
            menuCh.SetOsmanAchievement(AchievementIDs.FormosusOsmanUnlock);
            menuCh.SetHeavenAchievement(AchievementIDs.FormosusHeavenUnlock);

            var speaker = CreateScriptable<SpeakerData>();
            speaker.name = GetID("Formosus_SpeakerData");
            speaker.speakerName = "Formosus";
            speaker.portraitLooksCenter = false;
            speaker.portraitLooksLeft = true;
            speaker._emotionBundles = [];
            speaker._defaultBundle = new()
            {
                portrait = ch.characterSprite,
                dialogueSound = ch.dxSound,
                bundleTextColor = new(0.3725f, 0.0902f, 0.0902f)
            };
            LoadedAssetsHandler.LoadedSpeakers[speaker.name] = speaker;
        }
    }
}
