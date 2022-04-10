using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace ExpandTouchableRange
{
    [BepInPlugin(__GUID__, __NAME__, "1.0.1")]
    public class ExpandTouchableRange : BaseUnityPlugin
    {
        public const string __NAME__ = "ExpandTouchableRange";
        public const string __GUID__ = "com.hetima.dsp." + __NAME__;

        public static ConfigEntry<int> touchableRangeMultiplier;
        internal static float multiply;

        void Awake()
        {
            touchableRangeMultiplier = Config.Bind("General", "touchable_range_multiplier", 250,
                new ConfigDescription("multiplier percentage. min=100(%) max=999(%). original behavior is 100", new AcceptableValueRange<int>(100, 999)));

            touchableRangeMultiplier.Value = Math.Min(Math.Max(touchableRangeMultiplier.Value, 100), 999);
            multiply = (float)touchableRangeMultiplier.Value / 100f;
            touchableRangeMultiplier.SettingChanged += (sender, args) => {
                multiply = Math.Min(Math.Max(touchableRangeMultiplier.Value, 100), 999) / 100f;
            };

            new Harmony(__GUID__).PatchAll(typeof(GetObjectSelectDistance_Patch));
        }



        [HarmonyPatch(typeof(PlayerAction_Inspect), "GetObjectSelectDistance")]
        public static class GetObjectSelectDistance_Patch
        {
            public static void Postfix(ref float __result)
            {
                __result = __result * multiply;
            }
        }


    }
}
