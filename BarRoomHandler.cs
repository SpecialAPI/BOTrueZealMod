using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public class BarRoomHandler : BaseRoomHandler
    {
        public override BaseRoomItem[] RoomSelectables => [];

        public override bool IsPhisicallySolved => true;
        public override bool HasRoomAmbience => true;
        public override AmbienceType AmbienceType => AmbienceType.Secret;
        public override OverworldRoomState GetRoomState => OverworldRoomState.NPCNear;

        public override void PopulateRoom(IGameCheckData gameData, IMinimalRunInfoData runData, IMinimalZoneInfoData zoneData, int dataID)
        {
        }

        public override void PrepareRoom()
        {
        }
    }
}
