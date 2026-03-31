using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public static class Dialogues
    {
        public static DialogueSO ShellyBar;
        public static DialogueSO ShellyCombat;

        public static DialogueSO FormosusCombat;

        public static void Init()
        {
            // shelly
            var shellyProgram = Bundle.LoadAsset<YarnProgram>("ShellyKDialogue");
            var shellyDxID = GetID("ShellyK");
            LoadedDBsHandler.GetDialogueDB().AddOrChangeDialog(shellyDxID, shellyProgram);

            ShellyBar = CreateScriptable<DialogueSO>();
            ShellyBar.name = GetID("Bar_ShellyK_Dialogue");
            ShellyBar.m_DialogID = shellyDxID;
            ShellyBar.startNode = "TrueZeal_ShellyK_Bar_Start";
            ShellyBar.dialog = shellyProgram;
            LoadedAssetsHandler.LoadedDialogues[ShellyBar.name] = ShellyBar;

            ShellyCombat = CreateScriptable<DialogueSO>();
            ShellyCombat.name = GetID("Combat_ShellyK_Dialogue");
            ShellyCombat.m_DialogID = shellyDxID;
            ShellyCombat.startNode = "TrueZeal_ShellyK_Combat_Anton_Start";
            ShellyCombat.dialog = shellyProgram;
            LoadedAssetsHandler.LoadedDialogues[ShellyCombat.name] = ShellyCombat;

            // formosus
            var formosusProgram = Bundle.LoadAsset<YarnProgram>("FormosusDialogue");
            var formosusDxID = GetID("Formosus");
            LoadedDBsHandler.GetDialogueDB().AddOrChangeDialog(formosusDxID, formosusProgram);

            FormosusCombat = CreateScriptable<DialogueSO>();
            FormosusCombat.name = GetID("Combat_Formosus_Dialogue");
            FormosusCombat.m_DialogID = formosusDxID;
            FormosusCombat.startNode = "TrueZeal_Formosus_Quest_Complete";
            FormosusCombat.dialog = formosusProgram;
            LoadedAssetsHandler.LoadedDialogues[FormosusCombat.name] = FormosusCombat;
        }
    }
}
