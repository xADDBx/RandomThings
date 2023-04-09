using HarmonyLib;
using ModKit;
using ModKit.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
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
        public static ModEntry.ModLogger Mod;
        public static Dictionary<int, GameObject> objects = new();
        public static Settings settings;
        private static bool Load(ModEntry modEntry) {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnUnload = OnUnload;
            modEntry.OnShowGUI = (ModEntry me) => ModGUIShown = true;
            modEntry.OnHideGUI = (ModEntry me) => ModGUIShown = false;
            Mod = modEntry.Logger;
            settings = Settings.Load<Settings>(modEntry);
            onStart();
            HarmonyInstance = new Harmony(modEntry.Info.Id);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        private static bool OnUnload(ModEntry modEntry) {
            foreach (var k in objects.Keys) {
                objects[k].SafeDestroy();
            }
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
        static bool showInventories = false;
        static bool showDangerous = false;
        static void OnGUI(ModEntry modEntry) {
            LogSlider("Time Multiplier", ref settings.TimeMultiplier, 0.00001f, 10, 1, 5, "", AutoWidth());
            if (SaveLoadManager.Instance != null) {
                using (HorizontalScope()) {
                    Label("Changes the Amount of saves (probably per Slot) to keep. Default 10. Games are deleted when loading a save.".Cyan());

                    ValueAdjustorEditable("", () => settings.saveGameChainFileCap, (v) => {
                        settings.saveGameChainFileCap = v;
                        applySaveChange();
                    }, 1, 10, 50);
                }
            }
            if (Toggle("Show sort buttons in inventories", ref settings.showSortButtons)) {
                foreach (var obj in objects.Values) {
                    if (obj.name.Equals("CustomSortButton")) {
                        obj.SetActive(settings.showSortButtons);
                    }
                }
            }
            Toggle("Sort containers on Game Load", ref settings.sortOnGameLoad);
            Toggle("Sort containers when opening them", ref settings.sortContainerOnOpening);
            int sortmode = (int)settings.sortMode;
            if (ModKit.UI.SelectionGrid(ref sortmode, new string[] { "By Name Ascending", "By Name Descending", "By Resource Count Ascending", "By Resource Count Descending" }, 2)) {
                settings.sortMode = (Extensions.SortMode)sortmode;
            }
            DisclosureToggle("Show Open Inventories", ref showInventories);
            if (showInventories) {
                using (HorizontalScope()) {
                    Space(20);
                    using (VerticalScope()) {
                        var menu = MainGameScript.Instance.MainCamera.transform.Find("--- REGULAR MENUS Sort Order 15/Menu - InventoryUI");
                        foreach (Transform child in menu.transform) {
                            if (menu.gameObject.activeSelf) {
                                if (child.gameObject.activeSelf) {
                                    // Only handle Player Inventory and Normal Chests
                                    if (child.name.Equals("Chest Grid Container") || child.name.Equals("Avatar Inventory")) {
                                        using (HorizontalScope()) {
                                            Label(child.name.Green(), Width(200));

                                            InventoryGrid inv = child.GetComponentInChildren<InventoryGrid>();
                                            var container = inv.getInventory();
                                            if (container == null) {
                                                Label("Please reopen the container!".Red());
                                            } else {
                                                ActionButton("Sort Container", () => container.sort(settings.sortMode), Width(120));
                                                Space(-320);
                                                using (VerticalScope()) {
                                                    Label("");
                                                    using (HorizontalScope()) {
                                                        Label("Name".Cyan(), Width(200));
                                                        Label("Amount".Cyan(), Width(50));
                                                        Space(-250);
                                                    }
                                                    foreach (var slot in container.GetCurrentSlots()) {
                                                        using (HorizontalScope()) {
                                                            string name = slot.getName();
                                                            if (name != null) {
                                                                Label(name.Cyan(), Width(200));
                                                                Label(slot.StackSize.ToString());
                                                            } else {
                                                                Label("Empty".Orange());
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
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

        static void onStart() {
            applySaveChange();
        }

        static void applySaveChange() {
            SaveLoadManager.Instance.SetFieldValue("_saveGameChainFileCap", settings.saveGameChainFileCap);
        }

        public static bool ValueAdjustorEditable(string title, Func<int> get, Action<int> set, int increment = 1, int min = 0, int max = int.MaxValue, params GUILayoutOption[] options) {
            var changed = false;
            using (HorizontalScope()) {
                Label(title.cyan(), options);
                Space(15);
                var value = get();
                changed = ValueAdjuster(ref value, increment, min, max);
                if (changed) {
                    set(Math.Max(Math.Min(value, max), min));
                }
                Space(50);
                using (VerticalScope(Width(75))) {
                    ActionIntTextField(ref value, set, Width(75));
                    set(Math.Max(Math.Min(value, max), min));
                }
            }
            return changed;
        }
    }
}
