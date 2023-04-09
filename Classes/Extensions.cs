namespace RandomThings {
    public static class Extensions {

        public static SolidResourceHolder getInventory(this InventoryGrid inv) {
            return (inv.GetSourceOfElement(0) as SolidResourceHolder.SlotData)?.ResourceHolder;
        }
        public static void sort(this SolidResourceHolder container) {
            int backIndex = container.NumOfSlots - 1;
            for (int frontIndex = 0; frontIndex < container.NumOfOccupiedSlots; frontIndex++) {
                var slot = container._slotDataIndex[frontIndex];
                if (slot.StackSize == 0) {
                    while (backIndex >= 0) {
                        var slotBack = container._slotDataIndex[backIndex];
                        if (slotBack.StackSize > 0) {
                            slot.SwapSlotData(slotBack);
                            break;
                        }
                        backIndex -= 1;
                    }
                }
            }

            for (int start = 0; start < container.NumOfOccupiedSlots - 1; start++) {
                for (int runner = 0; runner < container.NumOfOccupiedSlots - (start + 1); runner++) {
                    var slot1 = container._slotDataIndex[runner];
                    var slot2 = container._slotDataIndex[runner + 1];
                    var result = slot1.getName().CompareTo(slot2.getName());
                    if (result > 0) {
                        slot1.SwapSlotData(slot2);
                    } else if (result == 0) {
                        var tmp = slot2.StackSize;
                        slot1.AddAmountToStackFrom(ref tmp, slot2.ResourceInstance.ResourceTypeIdentifier, slot2, slot2.ResourceInstance);
                        if (tmp == 0) {
                            // Adding some kind of elegant solution is beyond me right now.
                            container.sort();
                            return;
                        }
                    }
                }
            }
        }

        public static string getName(this SolidResourceHolder.SlotData slot) {
            return slot.ResourceInstance?.Name ?? null;
        }
    }
}
