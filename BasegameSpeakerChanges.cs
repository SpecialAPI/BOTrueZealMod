using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public static class BasegameSpeakerChanges
    {
        public static void Init()
        {
            var antonSpeaker = LoadedAssetsHandler.GetSpeakerData("Anton_SpeakerData");
            antonSpeaker._emotionBundles = antonSpeaker._emotionBundles.AddToArray(new()
            {
                emotion = "TrueZeal_Sad",
                bundle = new()
                {
                    portrait = LoadSprite("AntonSad"),
                    dialogueSound = antonSpeaker._defaultBundle.dialogueSound,
                    bundleTextColor = antonSpeaker._defaultBundle.bundleTextColor,
                }
            });

            var hansSpeaker = LoadedAssetsHandler.GetSpeakerData("Hans_SpeakerData");
            hansSpeaker._emotionBundles = hansSpeaker._emotionBundles.AddToArray(new()
            {
                emotion = "TrueZeal_Surprised",
                bundle = new()
                {
                    portrait = LoadSprite("HansSurprised"),
                    dialogueSound = hansSpeaker._defaultBundle.dialogueSound,
                    bundleTextColor = hansSpeaker._defaultBundle.bundleTextColor,
                }
            });
        }
    }
}
