using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    public static class Rarity
    {
        public static readonly RaritySO ExtremelyCommon     = New(99);
        public static readonly RaritySO Common              = New(10);
        public static readonly RaritySO Uncommon            = New(7);
        public static readonly RaritySO Rare                = New(5);
        public static readonly RaritySO VeryRare            = New(3);
        public static readonly RaritySO AbsurdlyRare        = New(1);
        public static readonly RaritySO Impossible          = New(0);
        public static readonly RaritySO ImpossibleNoReroll  = New(0, false);

        public static RaritySO New(int rarity, bool canBeRerolled = true)
        {
            var r = CreateScriptable<RaritySO>();
            r.rarityValue = rarity;
            r.canBeRerolled = canBeRerolled;

            return r;
        }
    }
}
