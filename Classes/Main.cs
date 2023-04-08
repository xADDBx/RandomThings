﻿using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using static ModKit.UI;

namespace RandomThings {
    public class Main {
        internal static Harmony HarmonyInstance;
        public static bool Enabled;
        public static Settings settings;
        private static bool Load(UnityModManager.ModEntry modEntry) {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnGUI;
            modEntry.OnUnload = OnUnload;
            settings = Settings.Load<Settings>(modEntry);
            HarmonyInstance = new Harmony(modEntry.Info.Id);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        private static bool OnUnload(UnityModManager.ModEntry modEntry) {
            HarmonyInstance.UnpatchAll(modEntry.Info.Id);
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            Enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Time Multiplier", GUILayout.ExpandWidth(false));
            GUILayout.Space(10);
            var tmp = settings.TimeMultiplier;
            settings.TimeMultiplier = GUILayout.HorizontalSlider(settings.TimeMultiplier, 0f, 10f, GUILayout.Width(300f));
            if (settings.TimeMultiplier != tmp)
                settings.changedTimeMultiplier = true;
            GUILayout.Label(settings.TimeMultiplier.ToString(), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            ActionButton("Save", () => {
                if (GameStatsManager.Instance != null && SaveLoadManager.Instance != null) {
                    GameStatsManager.Instance.TrySaveGameStatsToFile();
                    SaveLoadManager.Instance.TryWriteSaveGameDataToFile();
                }
            });
            GUILayout.Label(GameStatsManager.Instance.GetStatsInAString());
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            settings.Save(modEntry);
        }
    }
}
