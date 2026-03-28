using BOTrueZealMod.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Enemies
{
    public static class Bluey
    {
        public static SpawnRandomEnemyAnywhereEffect SpawnEffect;

        public static void Init()
        {
            var en = EnemyBuilder.NewEnemy("CadaverSynod_EN");
            en.SetName("Cadaver Synod");
            en.SetHealth(896, Pigments.Blue);
            en.AddPassives(CustomPassives.Anointed(1), Passives.Immortal, Passives.Withering);
            en.enemyLoot = new()
            {
                _lootableItems = [new()
                {
                    amount = 2,
                    isItemTreasure = false,
                    probability = 100
                }]
            };

            var sepulchre = LoadedAssetsHandler.LoadEnemy("Sepulchre_EN");
            var ow = LoadSprite("BlueyOW", new(0.5f, 0f));
            en.SetSprites(ow, ow, sepulchre.enemyOWCorpseSprite);
            en.SetSounds(sepulchre.damageSound, sepulchre.deathSound);
            en.SetEnemyPrefab("CadaverSynod_EFL Variant", "Giblets_CadaverSynod");
            var template = en.enemyTemplate;
            template._rootTransform = template.transform;
            template._locator = template.transform.Find("Locator").gameObject;
            template._renderer = template.transform.Find("Locator").Find("Sprite").GetComponent<SpriteRenderer>();
            template._animator = template.GetComponent<Animator>();
            template._UI3DLocation = template.transform.Find("3DUILocation");
            template._basicColor = sepulchre.enemyTemplate._basicColor;
            template._hoverColor = sepulchre.enemyTemplate._hoverColor;
            template._targetColor = sepulchre.enemyTemplate._targetColor;
            template._turnColor = sepulchre.enemyTemplate._turnColor;
            template._gibsEvent = "event:/Combat/Gibs/CBT_Gibs";

            SpawnEffect = CreateScriptable<SpawnRandomEnemyAnywhereEffect>();
            SpawnEffect._enemies = CreateBasegameSpawnPool();
            SpawnEffect._givesExperience = true;

            var formosusUnlockDialogue = CreateScriptable<DialogueSO>();
            formosusUnlockDialogue.name = GetID("Combat_Formosus_Dialogue");
            formosusUnlockDialogue.m_DialogID = GetID("Formosus");
            formosusUnlockDialogue.startNode = "TrueZeal_Formosus_Quest_Complete";
            formosusUnlockDialogue.dialog = Bundle.LoadAsset<YarnProgram>("FormosusDialogue");
            LoadedAssetsHandler.LoadedDialogues[formosusUnlockDialogue.name] = formosusUnlockDialogue;
            LoadedDBsHandler.GetDialogueDB().AddOrChangeDialog(formosusUnlockDialogue.m_DialogID, formosusUnlockDialogue.dialog);

            en.enterEffects = [Effects.Effect(null, SpawnEffect, 4)];
            en.exitEffects = [Effects.Effect(null, CreateScriptable<StartDialogueConversationEffect>(x => x._dialogue = formosusUnlockDialogue)).WithCondition(CreateScriptable<HasCustomAchievementEffectCondition>(x =>
            {
                x.achID = AchievementIDs.FormosusQuest;
                x.needsToBeUnlocked = false;
            }))];

            var exhumeName = "Exhume";
            var exhumeDesc = "Beckon forth as many enemies as possible.";
            var exhume = AbilityBuilder.NewAbility("Exhume_A")
            .SetBasicInformationEnemy(exhumeName, exhumeDesc)
            .SetEffects(new()
            {
                Effects.Effect(null, SpawnEffect, 4)
            })
            .SetIntents(new()
            {
                TargetIntent(Targets.Self, IntentType.Other_Spawn)
            })
            .SetVisuals(Animations.Birth, Targets.Self)
            .EnemyAbility(Rarity.Common, Priority.ExtremelySlow);

            en.SetAbilities([exhume]);

            en.AddToDatabase(false, false, false);
        }

        public static EnemySO[] CreateBasegameSpawnPool()
        {
            var enemyIDs = new string[]
            {
                "MudLung_EN",
                "MudLung_EN",
                "MunglingMudLung_EN",
                "Wringle_EN",
                "FlaMinGoa_EN",
                "JumbleGuts_Clotted_EN",
                "JumbleGuts_Flummoxing_EN",
                "JumbleGuts_Hollowing_EN",
                "JumbleGuts_Waning_EN",
                "Spoggle_Resonant_EN",
                "Spoggle_Ruminating_EN",
                "Spoggle_Spitfire_EN",
                "Spoggle_Writhing_EN",
                "Flarb_EN",
                "Voboola_EN",
                "Kekastle_EN",
                "MusicMan_EN",
                "MusicMan_EN",
                "Chordophone_EN",
                "Psaltery_EN",
                "Woodwind_EN",
                "Revola_EN",
                "ManicMan_EN",
                "WrigglingSacrifice_EN",
                "Conductor_EN",
                "NextOfKin_EN",
                "NextOfKin_EN",
                "InHerImage_EN",
                "InHisImage_EN",
                "ShiveringHomunculus_EN",
                "SkinningHomunculus_EN",
                "GigglingMinister_EN",
                "ChoirBoy_EN",
                "Xiphactinus_EN"
            };
            var enemies = new List<EnemySO>();

            foreach (var id in enemyIDs)
            {
                var enemy = LoadedAssetsHandler.GetEnemy(id);

                if(enemy == null)
                {
                    Debug.LogError($"Failed to get enemy {id}");
                    continue;
                }

                enemies.Add(enemy);
            }

            return [..enemies];
        }
    }
}
