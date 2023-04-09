using HarmonyLib;
using ModKit.Utility;
using System;
using System.Reflection;
using UnityEngine;
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
        public static ModEntry.ModLogger Mod;
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
            foreach (var k in Tweaks.customButtons.Keys) {
                Tweaks.customButtons[k].SafeDestroy();
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
        static bool showDangerous = false;
        static Avatar player;
        static void OnGUI(ModEntry modEntry) {
            LogSlider("Time Multiplier", ref settings.TimeMultiplier, 0.00001f, 10, 1, 5, "", AutoWidth());
            if (SaveLoadManager.Instance != null) {
                using (HorizontalScope()) {
                    ValueAdjustorEditable("", () => settings.saveGameChainFileCap, (v) => {
                        settings.saveGameChainFileCap = v;
                        applySaveChange();
                    }, 1, 10, 50, AutoWidth());
                    Label("Changes the Amount of saves (probably per Slot) to keep. Default 10. Games are deleted when loading a save.".Cyan());
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
                using (HorizontalScope()) {
                    Space(20);
                    using (VerticalScope()) {
                        if (player != null) {
                            PlayerInput inp = player.PlayerInput;
                            if (inp != null) {
                                try {
                                    var menu = MainGameScript.Instance.MainCamera.transform.Find("--- REGULAR MENUS Sort Order 15/Menu - InventoryUI");
                                    foreach (Transform child in menu.transform) {
                                        if (menu.gameObject.activeSelf) {
                                            if (child.gameObject.activeSelf) {
                                                using (HorizontalScope()) {
                                                    Label(child.name.Green());
                                                    if (child.name.Equals("Chest Grid Container") || child.name.Equals("Avatar Inventory")) {
                                                        InventoryGrid inv = child.GetComponentInChildren<InventoryGrid>();
                                                        var container = inv.getInventory();
                                                        if (container == null) {
                                                            Label("Please reopen the container!".Red());
                                                        } else {
                                                            ActionButton("Sort Container", () => container.sort());
                                                            using (HorizontalScope()) {
                                                                Space(20);
                                                                if (container != null) {
                                                                    using (VerticalScope()) {
                                                                        foreach (var slot in container.GetCurrentSlots()) {
                                                                            Label(slot.getName());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        } else {
                                            Label(child.name.Grey());
                                        }

                                    }
                                } catch (NullReferenceException e) {
                                    Mod.Error(e.ToString());
                                }
                            }
                        } else {
                            player = MainGameScript.Instance.PlayerAvatar;
                        }
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

        static void onStart() {
            applySaveChange();
            Tweaks.customButtons = new();
        }

        static void applySaveChange() {
            SaveLoadManager.Instance.SetFieldValue("_saveGameChainFileCap", settings.saveGameChainFileCap);
        }
    }
}
