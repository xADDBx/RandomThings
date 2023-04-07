using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace RandomThings {
    public class Settings : UnityModManager.ModSettings {
        public float TimeMultiplier = 1f;
        public bool changedTimeMultiplier = false;
        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(modEntry);
        }
    }
}
