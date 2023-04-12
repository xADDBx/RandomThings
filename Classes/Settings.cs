using UnityModManagerNet;

namespace RandomThings {
    public class Settings : UnityModManager.ModSettings {
        public float TimeMultiplier = 1f;
        public int selectedTab = 0;
        public int saveGameChainFileCap = 10;
        public Extensions.SortMode sortMode = Extensions.SortMode.byNameAsc;
        public bool sortOnGameLoad = false;
        public bool sortContainerOnOpening = false;
        public bool showSortButtons = false;
        public bool enableInvulnerability = false;
        public bool enableEverythingCostsNothing = false;
#if false
        public bool showCharacterOnMap = false;
#endif
        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
