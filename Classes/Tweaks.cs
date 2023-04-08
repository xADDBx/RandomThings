using HarmonyLib;

namespace RandomThings {
    internal static class Tweaks {
        public static Settings settings = Main.settings;

        [HarmonyPatch(typeof(MasterTimer), nameof(MasterTimer.Update))]
        private static class MasterTimer_Update_Patch {
            private static void Prefix(MasterTimer __instance) {
                if (!__instance.IsFastForwardingTowardsTarget && (settings.TimeMultiplier != 1f || settings.changedTimeMultiplier)) {
                    __instance.TimeMultiplier = settings.TimeMultiplier;
                }
            }
        }
    }
}
