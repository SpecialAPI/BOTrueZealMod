using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod
{
    public static class CustomAnimations
    {
        public static AttackVisualsSO BarFight;
        public static AttackVisualsSO Bell;
        public static AttackVisualsSO BodySnatcher;
        public static AttackVisualsSO Flirt;
        public static AttackVisualsSO Providence;
        public static AttackVisualsSO Relapse;
        public static AttackVisualsSO Scales;

        public static void Init()
        {
            BarFight        = Create("Barfight",        "");
            Bell            = Create("Bell",            "");
            BodySnatcher    = Create("BodySnatcher",    "");
            Flirt           = Create("Flirt",           "");
            Providence      = Create("Providence",      "");
            Relapse         = Create("Relapse",         "");
            Scales          = Create("Scales",          "");
        }

        private static AttackVisualsSO Create(string clipName, string sound, bool fullscreen = false)
        {
            var a = CreateScriptable<AttackVisualsSO>();
            a.name = GetID(clipName);
            a.animation = Bundle.LoadAsset<AnimationClip>(clipName);
            a.audioReference = sound;
            a.isAnimationFullScreen = fullscreen;

            return a;
        }
    }
}
