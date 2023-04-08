using HarmonyLib;
using ModKit.Utility;
using System.Reflection;
using UnityModManagerNet;
using static ModKit.UI;
using static UnityModManagerNet.UnityModManager;

namespace RandomThings {
#if DEBUG
    [EnableReloading]
#endif
    public class Main {
        internal static Harmony HarmonyInstance;
        public static bool Enabled;
        public static bool ModGUIShown = false;
        public static Settings settings;
        private static bool Load(ModEntry modEntry) {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUnload = OnUnload;
            modEntry.OnShowGUI = (ModEntry me) => ModGUIShown = true;
            modEntry.OnHideGUI = (ModEntry me) => ModGUIShown = false;
            settings = Settings.Load<Settings>(modEntry);
            onStart();
            HarmonyInstance = new Harmony(modEntry.Info.Id);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        private static bool OnUnload(ModEntry modEntry) {
            HarmonyInstance.UnpatchAll(modEntry.Info.Id);
            return true;
        }

        static bool OnToggle(ModEntry modEntry, bool value) {
            Enabled = value;
            return true;
        }

        static void OnSaveGUI(ModEntry modEntry) {
            settings.Save(modEntry);
        }


        static bool showGameStats = false;
        static void OnGUI(ModEntry modEntry) {
            if (LogSlider("Time Multiplier", ref settings.TimeMultiplier, 0.00001f, 10, 1, 5, "", AutoWidth()))
                settings.changedTimeMultiplier = true;
            if (SaveLoadManager.Instance != null) {
                using (HorizontalScope()) {
                    ValueAdjustorEditable("", () => settings.saveGameChainFileCap, (v) => {
                        settings.saveGameChainFileCap = v;
                        applySaveChange();
                    }, 1, 10, 50, AutoWidth());
                    Label("Changes the Amount of saves (probably per Slot) to keep. Default 10. Games are deleted when loading a save.".Cyan());
                }
            }
            ActionButton("Save", () => {
                if (GameStatsManager.Instance != null && SaveLoadManager.Instance != null) {
                    GameStatsManager.Instance.TrySaveGameStatsToFile();
                    SaveLoadManager.Instance.TryWriteSaveGameDataToFile();
                }
            });
            DisclosureToggle("Show Game Stats", ref showGameStats);
            if (showGameStats) {
                using (HorizontalScope()) {
                    Space(20);
                    Label(GameStatsManager.Instance.GetStatsInAString(), AutoWidth());
                }
            }
        }

        static void onStart() {
            applySaveChange();
        }

        static void applySaveChange() {
            SaveLoadManager.Instance.SetFieldValue("_saveGameChainFileCap", settings.saveGameChainFileCap);
        }
    }
}
