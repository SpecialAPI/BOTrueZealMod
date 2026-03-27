using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace BOTrueZealMod
{
    public class BarRoomHandler : BaseRoomHandler
    {
        public BaseRoomItem Shelly;

        public override BaseRoomItem[] RoomSelectables => [Shelly];
        public override bool IsPhisicallySolved => true;
        public override bool HasRoomAmbience => true;
        public override AmbienceType AmbienceType => AmbienceType.Secret;
        public override OverworldRoomState GetRoomState => OverworldRoomState.NPCNear;

        public TalkingEntityContentData entityData;

        public override void PopulateRoom(IGameCheckData gameData, IMinimalRunInfoData runData, IMinimalZoneInfoData zoneData, int dataID)
        {
            entityData = zoneData.GetTalkingEntityData(dataID);

            var dxRef = new DialogueDataReference(dataID, string.Empty);
            Shelly.SetClickData(Utils.startDialogNtf, dxRef);

            // only do this once per run
            if (entityData._hasBeenVisited)
                return;
            entityData._hasBeenVisited = true;

            // remove shelly if quest is completed
            entityData._isGone = gameData.DidCompleteQuest(QuestIDsE.ShellyKQuest);
        }

        public override void PrepareRoom()
        {
            if (!entityData.IsGone)
                return;

            DisableAndHideMainSelectables();
        }
    }
}
