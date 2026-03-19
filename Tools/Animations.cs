using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    public static class Animations
    {
        // Unique Fool Visuals - Nowak
        public static AttackVisualsSO Takedown = GetAnyAbility("Takedown_1_A").visuals;
        public static AttackVisualsSO Parry = GetAnyAbility("Parry_1_A").visuals;
        public static AttackVisualsSO Wrath = GetAnyAbility("Wrath_1_A").visuals;

        // Unique Fool Visuals - Anton
        public static AttackVisualsSO FingerGuns = GetAnyAbility("Blow_1_A").visuals;
        public static AttackVisualsSO MiddleFinger = GetAnyAbility("Insult_1_A").visuals;
        public static AttackVisualsSO Shank = GetAnyAbility("Shank_1_A").visuals;

        // Unique Fool Visuals - Kleiver
        public static AttackVisualsSO EarStab = GetAnyAbility("Cacophony_1_A").visuals;
        public static AttackVisualsSO CutOffFace = GetAnyAbility("Purify_1_A").visuals;

        // Unique Fool Visuals - Cranes
        public static AttackVisualsSO EatMyFlesh = GetAnyAbility("MyFlesh_1_A").visuals;
        public static AttackVisualsSO WasteAway = GetAnyAbility("WasteAway_1_A").visuals;

        // Unique Fool Visuals - Burnout
        public static AttackVisualsSO Buster = GetAnyAbility("Buster_1_A").visuals;
        public static AttackVisualsSO Decimation = GetAnyAbility("Decimation_1_A").visuals;

        // Unique Fool Visuals - Fennec
        public static AttackVisualsSO Quills = GetAnyAbility("Quills_1_A").visuals;
        public static AttackVisualsSO Thorns = GetAnyAbility("Thorns_1_A").visuals;

        // Generic Visuals - Bite
        public static AttackVisualsSO Nibble = GetAnyAbility("Nibble_A").visuals;
        public static AttackVisualsSO Chomp = GetAnyAbility("Chomp_A").visuals;
        public static AttackVisualsSO Gnaw = GetAnyAbility("Gnaw_A").visuals;

        // Generic Visuals - Gulp
        public static AttackVisualsSO Gulp = GetAnyAbility("Gulp_A").visuals;
        public static AttackVisualsSO MegaGulp = GetAnyAbility("Devour_A").visuals;

        // Generic Visuals - Burn
        public static AttackVisualsSO Burn = GetAnyAbility("Torch_1_A").visuals;
        public static AttackVisualsSO Oiled = GetAnyAbility("Oil_1_A").visuals;
        public static AttackVisualsSO OiledBurn = GetAnyAbility("WholeAgain_1_A").visuals;

        // Generic Visuals - Split/Connect
        public static AttackVisualsSO Split = GetAnyAbility("Amalgam_1_A").visuals;
        public static AttackVisualsSO Connection = GetAnyAbility("Entwined_1_A").visuals;

        // Generic Visuals - Smooch/Kiss
        public static AttackVisualsSO Smooch = GetAnyAbility("Blush_A").visuals;
        public static AttackVisualsSO Kiss = GetAnyAbility("TraumaBond_A").visuals;

        // Generic Visuals - Directional
        public static AttackVisualsSO Clobber = GetAnyAbility("Clobber_1_A").visuals;
        public static AttackVisualsSO PunchLeft = GetAnyAbility("Extrusion_A").visuals;
        public static AttackVisualsSO PunchRight = GetAnyAbility("Intrusion_A").visuals;
        public static AttackVisualsSO Stomp = GetAnyAbility("FallingSkies_A").visuals;

        // Generic Visuals - Slash
        public static AttackVisualsSO Slash = GetAnyAbility("FlayTheFlesh_A").visuals;
        public static AttackVisualsSO MegaSlash = GetAnyAbility("Domination_A").visuals;

        // Generic Visuals - Wriggle
        public static AttackVisualsSO Wriggle = GetAnyAbility("Wriggle_A").visuals;
        public static AttackVisualsSO MegaWriggle = GetAnyAbility("WrigglingWrath_A").visuals;
        public static AttackVisualsSO Concentration = GetAnyAbility("Concentration_A").visuals;

        // Generic Visuals - Scream
        public static AttackVisualsSO Scream = GetAnyAbility("Scream_1_A").visuals;
        public static AttackVisualsSO Bellow = GetAnyAbility("Bellow_A").visuals;

        // Generic Visuals - Punching
        public static AttackVisualsSO Bash = GetAnyAbility("Bash_A").visuals;
        public static AttackVisualsSO Contusion = GetAnyAbility("Contusion_1_A").visuals;

        // Generic Visuals - Bird
        public static AttackVisualsSO Rend = GetAnyAbility("RendTheRight_A").visuals;
        public static AttackVisualsSO Talons = GetAnyAbility("FlayTheFlesh_A").visuals;

        // Generic Visuals - Decay
        public static AttackVisualsSO Exsanguinate = GetAnyAbility("Exsanguinate_A").visuals;
        public static AttackVisualsSO Genesis = GetAnyAbility("Genesis_A").visuals;

        // Generic Visuals - Cry
        public static AttackVisualsSO Sob = GetAnyAbility("Sob_A").visuals;
        public static AttackVisualsSO Weep = GetAnyAbility("Weep_A").visuals;

        // Generic Visuals - Lick
        public static AttackVisualsSO HandLick = GetAnyAbility("PetroleumSweat_A").visuals;
        public static AttackVisualsSO Indulgence = GetAnyAbility("Indulgence_A").visuals;

        // Generic Visuals - Uncategorized
        public static AttackVisualsSO Crush = GetAnyAbility("Crush_A").visuals;
        public static AttackVisualsSO Slap = GetAnyAbility("Slap_A").visuals;
        public static AttackVisualsSO Resolve = GetAnyAbility("Resolve_1_A").visuals;
        public static AttackVisualsSO Poke = GetAnyAbility("PressurePoint_1_A").visuals;
        public static AttackVisualsSO Shield = GetAnyAbility("Entrenched_1_A").visuals;
        public static AttackVisualsSO Birth = GetAnyAbility("Repent_A").visuals;
        public static AttackVisualsSO Heal = GetAnyAbility("Mend_1_A").visuals;
        public static AttackVisualsSO Malpractice = GetAnyAbility("Malpractice_1_A").visuals;
        public static AttackVisualsSO Excommunicate = GetAnyAbility("Excommunicate_A").visuals;
        public static AttackVisualsSO Melt = GetAnyAbility("Boil_A").visuals;
        public static AttackVisualsSO Leech = GetAnyAbility("Leech_A").visuals;
        public static AttackVisualsSO Vomit = GetAnyAbility("Flood_A").visuals;

        // Unique Enemy Visuals - Far Shore
        public static AttackVisualsSO Mung = GetAnyAbility("Mungle_A").visuals;
        public static AttackVisualsSO Fandango = GetAnyAbility("Fandango_A").visuals;
        public static AttackVisualsSO Mandibles = GetAnyAbility("Mandibles_A").visuals;
        public static AttackVisualsSO HeartBreak = GetAnyAbility("HeartBreaker_A").visuals;

        // Unique Enemy Visuals - Orpheum
        public static AttackVisualsSO FeelTheRhythm = GetAnyAbility("FeelTheRhythm_A").visuals;
        public static AttackVisualsSO Strings = GetAnyAbility("StrikeAChord_A").visuals;
        public static AttackVisualsSO Conductor = GetAnyAbility("Crescendo_A").visuals;
        public static AttackVisualsSO EarPoke = GetAnyAbility("Connection_1_A").visuals;

        // Unique Enemy Visuals - Garden
        public static AttackVisualsSO RingABell = GetAnyAbility("RingABell_A").visuals;

        // Unique Enemy Visuals - Heir
        public static AttackVisualsSO Tamagoa = GetAnyAbility("TheMindAwakens_A").visuals;
        public static AttackVisualsSO HeirBorn = GetAnyAbility("HisChildIsBorn_A").visuals;
        public static AttackVisualsSO Womb = GetAnyAbility("ReturnToTheWomb_A").visuals;

        // Unique Enemy Visuals - Boss
        public static AttackVisualsSO Flex = GetAnyAbility("Flex_A").visuals;
        public static AttackVisualsSO Coil = GetAnyAbility("Coil_A").visuals;
        public static AttackVisualsSO Misery = GetAnyAbility("Misery_A").visuals;
        public static AttackVisualsSO MouldTheClay = GetAnyAbility("MouldTheClay_A").visuals;
        public static AttackVisualsSO OilSpill = GetAnyAbility("OilSpill_A").visuals;
        public static AttackVisualsSO SpearFishing = GetAnyAbility("Skewer_A").visuals;

        // Full Screen Visuals
        public static AttackVisualsSO Headshot = GetAnyAbility("Headshot_A").visuals;
        public static AttackVisualsSO Backbreaker = GetAnyAbility("Backbreaker_A").visuals;
        public static AttackVisualsSO Obsession = GetAnyAbility("Obsession_A").visuals;
        public static AttackVisualsSO BurningPassion = GetAnyAbility("BurningPassion_A").visuals;
        public static AttackVisualsSO Martyrdom = GetAnyAbility("Messiah_A").visuals;
        public static AttackVisualsSO Trauma = GetAnyAbility("Trauma_A").visuals;
        public static AttackVisualsSO Starvation = GetAnyAbility("Starvation_A").visuals;
        public static AttackVisualsSO RejectDeath = GetAnyAbility("RejectDeath_A").visuals;
        public static AttackVisualsSO MortalHorizon = GetAnyAbility("MortalHorizon_A").visuals;
        public static AttackVisualsSO ComeHome = GetAnyAbility("ComeHome_A").visuals;
        public static AttackVisualsSO DemonCore = ((LoadedAssetsHandler.GetWearable("DemonCore_SW") as PerformEffectWearable).effects.First(x => x.effect is AnimationVisualsEffect).effect as AnimationVisualsEffect)._visuals;
    }
}
