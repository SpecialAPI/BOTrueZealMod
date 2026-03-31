using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public static class Dialogues
    {
        public static DialogueSO ShellyBar;
        public static DialogueSO ShellyCombat;
        public static DialogueSO ShellyFreeFool;

        public static DialogueSO FormosusCombat;
        public static DialogueSO FormosusFreeFool;

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

            ShellyFreeFool = CreateScriptable<DialogueSO>();
            ShellyFreeFool.name = GetID("FreeFool_ShellyK_Dialogue");
            ShellyFreeFool.m_DialogID = shellyDxID;
            ShellyFreeFool.startNode = "TrueZeal_ShellyK_FreeFool_Start";
            ShellyFreeFool.dialog = shellyProgram;
            LoadedAssetsHandler.LoadedDialogues[ShellyFreeFool.name] = ShellyFreeFool;

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

            FormosusFreeFool = CreateScriptable<DialogueSO>();
            FormosusFreeFool.name = GetID("FreeFool_Formosus_Dialogue");
            FormosusFreeFool.m_DialogID = formosusDxID;
            FormosusFreeFool.startNode = "TrueZeal_Formosus_FreeFool_Start";
            FormosusFreeFool.dialog = formosusProgram;
            LoadedAssetsHandler.LoadedDialogues[FormosusFreeFool.name] = FormosusFreeFool;
        }
    }
}
