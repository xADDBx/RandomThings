using UnityModManagerNet;

namespace RandomThings {
    public class Settings : UnityModManager.ModSettings {
        public int selectedRawDataType = 0;
        public float TimeMultiplier = 1f;
        public float LootMultiplier = 1f;
        public int selectedTab = 0;
        public int saveGameChainFileCap = 10;
        public int maxChestStackSize = 50;
        public int maxCrateStackSize = 50;
        public Extensions.SortMode sortMode = Extensions.SortMode.byNameAsc;
        public bool sortOnGameLoad = false;
        public bool sortContainerOnOpening = false;
        public bool showSortButtons = false;
        public bool enableInvulnerability = false;
        public bool enableEverythingCostsNothing = false;
        public bool showCharacterOnMap = false;
        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
