using ModKit;
using System;
using UnityEngine;
using static ModKit.UI;

namespace RandomThings {
    public static class GUIUtil {
        public static bool ValueAdjustorEditable(string title, Func<int> get, Action<int> set, int increment = 1, int min = 0, int max = int.MaxValue, params GUILayoutOption[] options) {
            var changed = false;
            using (HorizontalScope()) {
                Label(title.cyan(), options);
                Space(15);
                var value = get();
                changed = ValueAdjuster(ref value, increment, min, max);
                if (changed) {
                    set(Math.Max(Math.Min(value, max), min));
                }
                Space(50);
                using (VerticalScope(Width(75))) {
                    ActionIntTextField(ref value, set, Width(75));
                    set(Math.Max(Math.Min(value, max), min));
                }
            }
            return changed;
        }
    }
}
