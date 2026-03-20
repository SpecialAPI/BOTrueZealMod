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
            en.enemyTemplate = sepulchre.enemyTemplate;

            SpawnEffect = CreateScriptable<SpawnRandomEnemyAnywhereEffect>();
            SpawnEffect._enemies = CreateBasegameSpawnPool();
            SpawnEffect._givesExperience = true;

            en.enterEffects = [Effects.Effect(null, SpawnEffect, 4)];
            en.priority = Priority.ExtremelySlow;

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
