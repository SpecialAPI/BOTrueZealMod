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
                return new()
                {
                    health = RankedValue(17, 19, 21, 24),
                    rankAbilities = LoadedAssetsHandler.GetCharcater("Boyle_CH").rankedData[rank].rankAbilities
                };
            });
            ch.AddToDatabase(true, false);
        }
    }
}
