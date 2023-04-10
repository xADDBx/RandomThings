using HarmonyLib;
using ModKit;
using ModKit.Utility;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using static ModKit.UI;
using static UnityModManagerNet.UnityModManager;

namespace RandomThings {
#if DEBUG
    [EnableReloading]
#endif
    internal static class Main {
        public static Harmony HarmonyInstance;
        public static bool Enabled;
        public static ModEntry.ModLogger Mod;
        public static Dictionary<int, GameObject> objects = new();
        public static Settings settings;
        public static bool Load(ModEntry modEntry) {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUnload = OnUnload;
            Mod = modEntry.Logger;
            settings = Settings.Load<Settings>(modEntry);
            onStart();
            HarmonyInstance = new Harmony(modEntry.Info.Id);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        public static bool OnUnload(ModEntry modEntry) {
            foreach (var k in objects.Keys) {
                objects[k].SafeDestroy();
            }
            HarmonyInstance.UnpatchAll(modEntry.Info.Id);
            return true;
        }

        public static bool OnToggle(ModEntry modEntry, bool value) {
            Enabled = value;
            return true;
        }

        public static void OnSaveGUI(ModEntry modEntry) {
            settings.Save(modEntry);
        }
        public static void OnGUI(ModEntry modEntry) {
            TabBar(ref settings.selectedTab,
                () => Space(25),
                new NamedAction("Inventory", () => InventoryUI.OnGUI()),
                new NamedAction("Other", () => OtherUI.OnGUI()));
        }
        public static void onStart() {
            applySaveChange();
        }
        public static void applySaveChange() {
        }
    }
}
