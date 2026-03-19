using BOTrueZealMod.CustomTargeting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    public static class Targets
    {
        public static BaseCombatTargettingSO Self = CreateScriptable<Targetting_BySlot_Index>(x =>
        {
            x.allSelfSlots = false;
            x.getAllies = true;
            x.slotPointerDirections = new int[]
            {
                0
            };
        });

        public static BaseCombatTargettingSO SelfAll = CreateScriptable<Targetting_BySlot_Index>(x =>
        {
            x.allSelfSlots = true;
            x.getAllies = true;
            x.slotPointerDirections = new int[]
            {
                0
            };
        });

        public static BaseCombatTargettingSO Front = CreateScriptable<Targetting_BySlot_Index>(x =>
        {
            x.allSelfSlots = true;
            x.getAllies = false;
            x.slotPointerDirections = new int[]
            {
                0
            };
        });

        public static BaseCombatTargettingSO AllySide = CreateScriptable<GenericTargetting_BySlot_Index>(x =>
        {
            x.getAllies = true;
            x.slotPointerDirections = new int[]
            {
                0,
                1,
                2,
                3,
                4
            };
        });

        public static BaseCombatTargettingSO OpponentSide = CreateScriptable<GenericTargetting_BySlot_Index>(x =>
        {
            x.getAllies = false;
            x.slotPointerDirections = new int[]
            {
                0,
                1,
                2,
                3,
                4
            };
        });

        public static BaseCombatTargettingSO AllAllySlots = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = true;
            x.getAllUnitSlots = true;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO AllAllies = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = true;
            x.getAllUnitSlots = false;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO OtherAllySlots = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = true;
            x.getAllUnitSlots = true;
            x.ignoreCastSlot = true;
        });

        public static BaseCombatTargettingSO OtherAllies = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = true;
            x.getAllUnitSlots = false;
            x.ignoreCastSlot = true;
        });

        public static BaseCombatTargettingSO AllOpponentSlots = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = false;
            x.getAllUnitSlots = true;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO AllOpponents = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = false;
            x.getAllUnitSlots = false;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO FurthestOpponents = CreateScriptable<TargetingByDistance>(x =>
        {
            x.getFurthest = true;

            x.getAllies = false;
            x.getAllUnitSlots = false;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO Relative(bool allies, params int[] offsets)
        {
            return CreateScriptable<Targetting_BySlot_Index>(x =>
            {
                x.allSelfSlots = true;
                x.getAllies = allies;
                x.slotPointerDirections = offsets;
            });
        }

        public static BaseCombatTargettingSO Absolute(bool allies, params int[] positions)
        {
            return CreateScriptable<GenericTargetting_BySlot_Index>(x =>
            {
                x.getAllies = allies;
                x.slotPointerDirections = positions;
            });
        }

        public static BaseCombatTargettingSO UnitsWithStatus(bool allies, StatusEffectType status, bool allSlots = false)
        {
            return CreateScriptable<TargetingByStatus>(x =>
            {
                x.getStatus = true;
                x.specificStatusOnly = true;
                x.specificStatus = [status];

                x.getAllies = allies;
                x.getAllUnitSlots = allSlots;
                x.ignoreCastSlot = false;
            });
        }
    }
}
