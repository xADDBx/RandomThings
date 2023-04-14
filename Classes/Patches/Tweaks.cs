﻿using HarmonyLib;
using System;
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

        [HarmonyPatch(typeof(UIManager), nameof(UIManager.ShowMenu), new Type[] { typeof(FullscreenUIWindowManaged.FullscreenMenuId), typeof(object) })]
        public static class UIManager_ShowMenu_Patch {
            public static void updatePosition(GameObject playerMarker) {
                Vector3 pos = MainGameScript.Instance.PlayerAvatar.transform.localPosition;
                Vector3 posStraight = Matrix4x4.Rotate(Quaternion.Euler(0, -45, 0)).MultiplyPoint(pos);
                // Magic numbers found by testing
                posStraight.x *= 2.4644f;
                posStraight.y = posStraight.z * 2.4333f;
                posStraight.z = 0;
                playerMarker.transform.localPosition = posStraight + new Vector3(-149.25f, 280.5f, 0);
                foreach (Transform child in playerMarker.transform) {
                    child.gameObject.SetActive(true);
                }
            }
            public static void createPlayerMarker() {
                GameObject ShopPin = MainGameScript.Instance.MainCamera.transform.Find("--- REGULAR MENUS Sort Order 15/Menu - Journal/Map/LandmarkLocations/Landmark Location Shop").gameObject;
                GameObject playerMarker = GameObject.Instantiate(ShopPin, ShopPin.transform.parent);
                playerMarker.name = "Landmark Location Player";
                playerMarker.transform.localScale = new Vector3(0.7f, 0.7f);
                Main.objects.Add(playerMarker.GetHashCode(), playerMarker);
                foreach (Transform child in playerMarker.transform) {
                    if (child.name.Equals("ButtonText (TMP)")) {
                        GameObject.DestroyImmediate(child.GetComponent<I2.Loc.Localize>());
                        child.GetComponent<TMPro.TextMeshProUGUI>().text = "Player";
                    }
                }
                GameObject zoomButton = MainGameScript.Instance.MainCamera.transform.Find("--- REGULAR MENUS Sort Order 15/Menu - Journal/Map/ButtonPlaqueRoundImg (TMP) Label/ButtonPlaqueRoundImg (TMP)").gameObject;
                Button button = zoomButton.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                // Needs better implementation (pops in immediately instead of waiting for animation to finish)
                button.onClick.AddListener(() => {
                    bool turnOn = settings.showCharacterOnMap && !playerMarker.activeSelf;
                    playerMarker.SetActive(turnOn);
                    if (turnOn) updatePosition(playerMarker);
                });
                updatePosition(playerMarker);
            }
            private static void Prefix(FullscreenUIWindowManaged.FullscreenMenuId menuId) {
                if (menuId == FullscreenUIWindowManaged.FullscreenMenuId.Journal) {
                    if (settings.showCharacterOnMap) {
                        Avatar player = MainGameScript.Instance.PlayerAvatar;
                        if (player != null) {
                            foreach (GameObject obj in Main.objects.Values) {
                                if (obj.name.Equals("Landmark Location Player")) {
                                    updatePosition(obj);
                                    return;
                                }
                            }
                            createPlayerMarker();
                        }
                    }
                }
            }
        }

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
