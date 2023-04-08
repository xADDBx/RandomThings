using UnityModManagerNet;

namespace RandomThings {
    public class Settings : UnityModManager.ModSettings {
        public float TimeMultiplier = 1f;
        public int saveGameChainFileCap = 10;
        public bool changedTimeMultiplier = false;
        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
