using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace RandomThings {
    public static class Tweaks {
        public static Settings settings => Main.settings;

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
                    if (settings.showSortButtons) {
                        var inventoryMenu = inv.Find("HeaderPlank");
                        if (inventoryMenu != null) {
                            if (inventoryMenu.Find("CustomSortButton") == null) {
                                GameObject SkinButton = inventoryMenu.transform.Find("ButtonPlaqueRoundImg (TMP) HUD Navigation back").gameObject;
                                GameObject myButton = GameObject.Instantiate(SkinButton, inventoryMenu.transform);
                                myButton.name = "CustomSortButton";
                                Main.objects[myButton.GetHashCode()] = myButton;
                                myButton.transform.localPosition = new Vector3(-150, 5.5f, 0);
                                myButton.transform.Find("Icon").gameObject.SetActive(false);
                                GameObject.DestroyImmediate(myButton.GetComponent<Button>());
                                Button b = myButton.AddComponent<Button>();
                                b.onClick.AddListener(() => __instance.getInventory().sort(settings.sortMode));
                            }
                        }
                    }
                    if (settings.sortContainerOnOpening) {
                        __instance.getInventory().sort(settings.sortMode);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SolidResourceHolder), nameof(SolidResourceHolder.LoadContentsSaveGameData))]
        private static class SolidResourceHolder_LoadContentsSaveGameData_Patch {
            private static void Postfix(SolidResourceHolder __instance) {
                if (settings.sortOnGameLoad) {
                    if (__instance.name.Equals("Chest(Clone)") || __instance.name.Equals("Avatar")) {
                        __instance.sort(settings.sortMode);
                    }
                }
            }
        }

#if false
        [HarmonyPatch(typeof(UIManager), nameof(UIManager.ShowMenu), new Type[] { typeof(FullscreenUIWindowManaged.FullscreenMenuId), typeof(object) })]
        private static class UIManager_ShowMenu_Patch {
            private static void Prefix(FullscreenUIWindowManaged.FullscreenMenuId menuId) {
                if (menuId == FullscreenUIWindowManaged.FullscreenMenuId.Journal) {
                    if (settings.showCharacterOnMap) {
                        Avatar player = MainGameScript.Instance.PlayerAvatar;
                        if (player != null) {
                            foreach (GameObject obj in Main.objects.Values) {
                                if (obj.name.Equals("Landmark Location Player")) {
                                    obj.transform.position = -obj.transform.InverseTransformPoint(player.CurrentPosition);
                                    foreach (Transform child in obj.transform) {
                                        child.gameObject.SetActive(true);
                                    }

                                    return;
                                }
                            }
                            GameObject ShopPin = MainGameScript.Instance.MainCamera.transform.Find("--- REGULAR MENUS Sort Order 15/Menu - Journal/Map/LandmarkLocations/Landmark Location Shop").gameObject;
                            GameObject playerMarker = GameObject.Instantiate(ShopPin, ShopPin.transform.parent);
                            playerMarker.name = "Landmark Location Player";
                            Main.objects.Add(playerMarker.GetHashCode(), playerMarker);
                            //Shop -> (0, 0, 0) -> (622.75, 809.50, 0.00);
                            //Teleport Shop -> (13.8, 0, -13.8) -> (674.20, 800.35, 0.00);
                            //Teleport City -> (-90.42, 0.00, -179.86) -> (778.90 347.75, 0.00);
                            //Bridge -> (-13.44, 0.00, 5.96) -> (549.10, 780.70, 0.00);
                            foreach (Transform child in playerMarker.transform) {
                                if (child.name.Equals("ButtonText(TMP)")) {
                                    child.GetComponent<I2.Loc.Localize>().gameObject.SetActive(false);
                                    child.GetComponent<TMPro.TextMeshProUGUI>().text = "Player Marker";
                                }
                                child.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
#endif
        [HarmonyPatch(typeof(PlayerPurchaseCost), nameof(PlayerPurchaseCost.TryDoPayment))]
        private static class PlayerPurchaseCost_TryDoPayment_Patch {
            private static bool Prefix() {
                if (settings.enableEverythingCostsNothing) {
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(RadialSwipe), nameof(RadialSwipe.DoDamage))]
        private static class RadialSwipe_DoDamage_Patch {
            private static bool Prefix() {
                if (settings.enableInvulnerability) {
                    return false;
                }
                return true;
            }
        }
    }
}
