using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace FermenterControl
{
    internal static class DurationApplier
    {
        private static readonly Dictionary<int, float> VanillaSeconds = new Dictionary<int, float>();

        internal static void Apply(Fermenter fermenter)
        {
            if (fermenter == null)
            {
                return;
            }

            int id = fermenter.GetInstanceID();
            if (!VanillaSeconds.ContainsKey(id))
            {
                VanillaSeconds[id] = fermenter.m_fermentationDuration;
            }

            fermenter.m_fermentationDuration = FermentTime.ResolveSeconds(
                Plugin.ConfiguredMinutes(),
                VanillaSeconds[id]);
        }

        internal static void ApplyAll()
        {
            Fermenter[] fermenters = Object.FindObjectsByType<Fermenter>(FindObjectsSortMode.None);
            for (int i = 0; i < fermenters.Length; i++)
            {
                Apply(fermenters[i]);
            }
        }
    }

    [HarmonyPatch(typeof(Fermenter), "Awake")]
    internal static class FermenterAwakePatch
    {
        private static void Postfix(Fermenter __instance)
        {
            DurationApplier.Apply(__instance);
        }
    }

    [HarmonyPatch(typeof(Fermenter), nameof(Fermenter.GetHoverText))]
    internal static class FermenterHoverPatch
    {
        private static readonly MethodInfo StatusMethod = AccessTools.Method(typeof(Fermenter), "GetStatus");
        private static readonly MethodInfo ElapsedMethod = AccessTools.Method(typeof(Fermenter), "GetFermentationTime");

        private static bool _loggedHover;

        private static void Postfix(Fermenter __instance, ref string __result)
        {
            if (__instance == null || string.IsNullOrEmpty(__result))
            {
                return;
            }

            try
            {
                if (!IsBrewing(__instance))
                {
                    return;
                }

                float elapsed = Elapsed(__instance);
                __result = FermentTime.WithRemaining(
                    __result,
                    true,
                    elapsed,
                    __instance.m_fermentationDuration,
                    Plugin.ConfiguredColor());
            }
            catch (System.Exception ex)
            {
                if (!_loggedHover)
                {
                    _loggedHover = true;
                    Plugin.LogWarning("Could not add the fermenter time: " + ex.Message);
                }
            }
        }

        private static bool IsBrewing(Fermenter fermenter)
        {
            if (StatusMethod == null)
            {
                return false;
            }

            object status = StatusMethod.Invoke(fermenter, null);
            if (status == null)
            {
                return false;
            }

            string name = status.ToString();
            return name == "Fermenting" || name == "Exposed";
        }

        private static float Elapsed(Fermenter fermenter)
        {
            if (ElapsedMethod == null)
            {
                return 0f;
            }

            object value = ElapsedMethod.Invoke(fermenter, null);
            if (value is double seconds)
            {
                return (float)seconds;
            }

            if (value is float secondsFloat)
            {
                return secondsFloat;
            }

            return 0f;
        }
    }
}
