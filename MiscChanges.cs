using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public static class MiscChanges
    {
        public static void Init()
        {
            LoadedAssetsHandler.GetWearable("MedicalLeeches_SW").shopPrice = 10;
        }
    }
}
