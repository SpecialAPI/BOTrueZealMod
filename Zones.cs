using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public static class Zones
    {
        public static ZoneBGDataBaseSO Easy1;
        public static ZoneBGDataBaseSO Easy2;
        public static ZoneBGDataBaseSO Easy3;

        public static ZoneBGDataBaseSO Hard1;
        public static ZoneBGDataBaseSO Hard2;
        public static ZoneBGDataBaseSO Hard3;

        public static void Init()
        {
            Easy1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_01") as ZoneBGDataBaseSO;
            Easy2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_02") as ZoneBGDataBaseSO;
            Easy3 = LoadedAssetsHandler.GetZoneDB("ZoneDB_03") as ZoneBGDataBaseSO;

            Hard1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01") as ZoneBGDataBaseSO;
            Hard2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_02") as ZoneBGDataBaseSO;
            Hard3 = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_03") as ZoneBGDataBaseSO;
        }
    }
}
