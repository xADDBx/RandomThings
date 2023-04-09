using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace RandomThings {
    internal static class Tweaks {
        public static Settings settings = Main.settings;

        [HarmonyPatch(typeof(MasterTimer), nameof(MasterTimer.Update))]
        private static class MasterTimer_Update_Patch {
            private static void Prefix(MasterTimer __instance) {
                if (!__instance.IsFastForwardingTowardsTarget && settings.TimeMultiplier != 1f) {
                    __instance.TimeMultiplier = settings.TimeMultiplier;
                }
            }
        }


        [HarmonyPatch(typeof(InventoryGrid), nameof(InventoryGrid.InitializeGridWithElements))]
        private static class InventoryGrid_InitializeGridWithElements_Patch {
            private static void Postfix(InventoryGrid __instance) {
                Transform inv = null;
                if (__instance.gameObject.name.Equals("Inventory Grid Avatar")) {
                    inv = __instance.gameObject.transform.parent.parent;
                } else if (__instance.gameObject.name.Equals("Inventory Grid SmallChest Variant")) {
                    inv = __instance.gameObject.transform.parent;
                }
                if (inv != null) {
                    var inventoryMenu = inv.Find("HeaderPlank");
                    if (inventoryMenu != null) {
                        if (inventoryMenu.Find("CustomSortButton") == null) {
                            Main.Mod.Log("Entered");
                            GameObject SkinButton = inventoryMenu.transform.Find("ButtonPlaqueRoundImg (TMP) HUD Navigation back").gameObject;
                            GameObject myButton = GameObject.Instantiate(SkinButton, inventoryMenu.transform);
                            myButton.name = "CustomSortButton";
                            Main.objects[myButton.GetHashCode()] = myButton;
                            myButton.transform.localPosition = new Vector3(-150, 5.5f, 0);
                            myButton.transform.Find("Icon").gameObject.SetActive(false);
                            GameObject.DestroyImmediate(myButton.GetComponent<Button>());
                            Button b = myButton.AddComponent<Button>();
                            b.onClick.AddListener(() => __instance.getInventory().sort());
                        }
                    }
                }
            }
        }

        // Inventories are random after loading no more
        [HarmonyPatch(typeof(SolidResourceHolder), nameof(SolidResourceHolder.LoadContentsSaveGameData))]
        private static class SolidResourceHolder_LoadContentsSaveGameData_Patch {
            private static void Postfix(SolidResourceHolder __instance) {
                if (__instance.name.Equals("Chest(Clone)") || __instance.name.Equals("Avatar")) {
                    __instance.sort();
                }
            }
        }
    }
}
