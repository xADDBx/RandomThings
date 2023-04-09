using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

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

        public static Dictionary<InventoryGrid, GameObject> customButtons;
        [HarmonyPatch(typeof(InventoryGrid), nameof(InventoryGrid.InitializeGridWithElements))]
        private static class InventoryGrid_InitializeGridWithElements_Patch {
            private static void Postfix(InventoryGrid __instance) {
#if false
                Transform ancestor = element.transform.parent.parent.parent.parent.parent;
                var grid = ancestor.GetComponentInChildren<InventoryGrid>();
                if (grid != null) {
                    Extensions.gridToInv[grid] = __instance.ResourceHolder;
                    if (customButtons.GetValueOrDefault(grid, null) == null) {
                        Transform SkinButton = MainGameScript.Instance.MainCamera.transform.Find("--- REGULAR MENUS Sort Order 15/Menu - InventoryUI/Avatar Inventory/HeaderPlank/");
                        customButtons[grid] = GameObject.Instantiate(SkinButton.gameObject, SkinButton.parent);
                        customButtons[grid].transform.Find("Icon").gameObject.SetActive(false);
                        customButtons[grid].transform.localPosition = new Vector3(-150, 5.5f, 0);
                        customButtons[grid].GetComponent<Button>().SafeDestroy();
                        Button b = customButtons[grid].AddComponent<Button>();
                        b.onClick.AddListener(() => __instance.ResourceHolder.sort());
                    }
                } else {
                    Mod.Log(ancestor.gameObject.ToString());
                }   
#endif
            }
        }
    }
}
