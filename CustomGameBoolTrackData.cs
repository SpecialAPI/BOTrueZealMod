using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace BOTrueZealMod
{
    public class CustomGameBoolTrackData : UnlockTrack_Data
    {
        public string boolDataKey;

        public string locID;
        public string defaultText;

        public override bool IsTrackDataAvailable(GameInformationHolder holder)
        {
            return holder.Game.GetBoolData(boolDataKey);
        }

        public override string GetDescription(GameInformationHolder holder)
        {
            return LocUtils.LocDB.GetCustomUIData(locID, defaultText);
        }
    }
}
