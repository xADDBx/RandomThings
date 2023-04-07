using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomThings {
    internal static class Tweaks {
        public static Settings settings = Main.settings;

        [HarmonyPatch(typeof(MasterTimer), nameof(MasterTimer.Update))]
        private static class Calender_Add_Patch {
            private static void Prefix(MasterTimer __instance) {
                if (settings.TimeMultiplier != 1f || settings.changedTimeMultiplier) {
                    __instance.TimeMultiplier = settings.TimeMultiplier;
                }
            }
        }
    }
}
