using BepInEx;
using BepInEx.Configuration;
using BOTrueZealMod.Characters;
using System;

namespace BOTrueZealMod
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string MOD_GUID = "157.TrueZeal";
        public const string MOD_NAME = "True Zeal";
        public const string MOD_VERSION = "1.4.2";
        public const string MOD_PREFIX = "TrueZeal";

        public static Harmony HarmonyInstance = new(MOD_GUID);
        public static Assembly ModAssembly;
        public static ConfigFile ModConfig;
        public static AssetBundle Bundle;

        public void Awake()
        {
            #region misc setup
            ModAssembly = Assembly.GetExecutingAssembly();
            ModConfig = Config;

            GlossaryStuffAdder.glossaryDB = Resources.FindObjectsOfTypeAll<GlossaryDataBase>().FirstOrDefault();
            infoHolder = Resources.FindObjectsOfTypeAll<GameInformationHolder>().FirstOrDefault();
            if (infoHolder != null)
                itemPool = infoHolder.ItemPoolDB;
            if (itemPool == null)
                itemPool = Resources.FindObjectsOfTypeAll<ItemPoolDataBaseSO>().FirstOrDefault();

            Pigments.Init();
            Passives.Init();

            HarmonyInstance.PatchAll();
            Magic.ExtendAllEnums();
            #endregion

            ShellyK.Init();
        }
    }
}
