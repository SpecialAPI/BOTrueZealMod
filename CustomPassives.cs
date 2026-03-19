using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public static class CustomPassives
    {
        private static Dictionary<int, BasePassiveAbilitySO> Anointeds = [];

        private static Sprite AnointedIcon;

        public static void Init()
        {
            AnointedIcon = LoadSprite("Anointed");
        }

        public static BasePassiveAbilitySO Anointed(int count)
        {
            return Anointeds.GetOrCreate(count, _ =>
            {
                var x = CreateScriptable<PerformEffectPassiveAbility>();

                x._passiveName = $"Anointed ({count})";
                x.passiveIcon = AnointedIcon;
                x.type = PassiveAbilityTypesE.Anointed;
                x._triggerOn = [TriggerCalls.OnTurnStart];
                x._characterDescription = "On turn start, this party member blesses the opposing enemy.";
                x._enemyDescription = "On turn start, this enemy blesses the opposing party members.";
                x.conditions = [];
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.effects = [Effects.Effect(Targets.OpposingSlot, CreateScriptable<ApplyDivineProtectionEffect>(), count)];

                x.name = $"Anointed_{count}_PA";

                return x;
            }, false);
        }
    }
}
