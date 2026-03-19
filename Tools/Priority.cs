using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    public static class Priority
    {
        public static readonly PrioritySO ExtremelySlow  = New(-3);
        public static readonly PrioritySO VerySlow       = New(-2);
        public static readonly PrioritySO Slow           = New(-1);
        public static readonly PrioritySO Normal         = New(0);
        public static readonly PrioritySO Fast           = New(1);
        public static readonly PrioritySO VeryFast       = New(2);
        public static readonly PrioritySO ExtremelyFast  = New(3);
        public static readonly PrioritySO Player         = New(1000);

        public static PrioritySO New(int value)
        {
            var p = CreateScriptable<PrioritySO>();
            p.priorityValue = value;

            return p;
        }
    }
}
