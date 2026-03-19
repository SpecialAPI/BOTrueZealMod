using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    public static class Effects
    {
        public static EffectInfo Effect(BaseCombatTargettingSO targets, EffectSO effect, int var = 0)
        {
            if (effect is AnimationVisualsEffect vis)
                vis._animationTarget = targets;

            if (effect is AnimationVisualsIfUnitEffect vis2)
                vis2._animationTarget = targets;

            return new()
            {
                effect = effect,
                targets = targets,
                entryVariable = var,
            };
        }

        public static EffectInfo WithCondition(this EffectInfo inf, EffectConditionSO condition)
        {
            inf.condition = condition;

            return inf;
        }
    }
}
