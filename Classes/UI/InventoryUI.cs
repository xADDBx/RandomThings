﻿using ModKit.Utility;
using UnityEngine;
using static ModKit.UI;

namespace RandomThings {
    public static class InventoryUI {
        public static Settings settings => Main.settings;
        static bool showInventories = false;
        public static void OnGUI() {
            if (Toggle("Show sort buttons in inventories", ref settings.showSortButtons)) {
                foreach (var obj in Main.objects.Values) {
                    if (obj.name.Equals("CustomSortButton")) {
                        obj.SetActive(settings.showSortButtons);
                    }
                }
            }
            Toggle("Sort containers on Game Load", ref settings.sortOnGameLoad);
            Toggle("Sort containers when opening them", ref settings.sortContainerOnOpening);
            int sortmode = (int)settings.sortMode;
            if (SelectionGrid(ref sortmode, new string[] { "By Name Ascending", "By Name Descending", "By Resource Count Ascending", "By Resource Count Descending" }, 2)) {
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
        }
    }
}
