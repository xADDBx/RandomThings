using ModKit.Utility;
using static ModKit.UI;

namespace RandomThings {
    public static class OtherUI {
        public static Settings settings => Main.settings;
        static bool showGameStats = false;
        static bool showDangerous = false;
        public static void OnGUI() {
            LogSlider("Time Multiplier", ref settings.TimeMultiplier, 0.00001f, 10, 1, 5, "", AutoWidth());
            if (SaveLoadManager.Instance != null) {
                using (HorizontalScope()) {
                    Label("Changes the Amount of saves (probably per Slot) to keep. Default 10. Games are deleted when loading a save.".Cyan());
                    ValueAdjustorEditable("", () => settings.saveGameChainFileCap, (v) => {
                        settings.saveGameChainFileCap = v;
                        Main.applySaveChange();
                    }, 1, 10, 50);
                }
            }
            DisclosureToggle("Show Game Stats", ref showGameStats);
            if (showGameStats) {
                using (HorizontalScope()) {
                    Space(20);
                    Label(GameStatsManager.Instance.GetStatsInAString(), AutoWidth());
                }
            }
            DisclosureToggle("Dangerous", ref showDangerous);
            if (showDangerous) {
                ActionButton("Save", () => {
                    if (GameStatsManager.Instance != null && SaveLoadManager.Instance != null) {
                        GameStatsManager.Instance.TrySaveGameStatsToFile();
                        SaveLoadManager.Instance.TryWriteSaveGameDataToFile();
                    }
                });
            }
        }
    }
}
