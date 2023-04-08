using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;
using static ModKit.UI;

namespace RandomThings {
    public class Main {
        internal static Harmony HarmonyInstance;
        public static bool Enabled;
        public static Settings settings;
        private static bool Load(UnityModManager.ModEntry modEntry) {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnGUI;
            modEntry.OnUnload = OnUnload;
            settings = Settings.Load<Settings>(modEntry);
            HarmonyInstance = new Harmony(modEntry.Info.Id);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        private static bool OnUnload(UnityModManager.ModEntry modEntry) {
            HarmonyInstance.UnpatchAll(modEntry.Info.Id);
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            Enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry) {
            if (LogSlider("Time Multiplier", ref settings.TimeMultiplier, 0.00001f, 10, 1, 5, "", AutoWidth()))
                settings.changedTimeMultiplier = true;
            ActionButton("Save", () => {
                if (GameStatsManager.Instance != null && SaveLoadManager.Instance != null) {
                    GameStatsManager.Instance.TrySaveGameStatsToFile();
                    SaveLoadManager.Instance.TryWriteSaveGameDataToFile();
                }
            });
            Label(GameStatsManager.Instance.GetStatsInAString(), AutoWidth());
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
