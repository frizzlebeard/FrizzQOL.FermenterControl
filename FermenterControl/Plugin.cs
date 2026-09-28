using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace FermenterControl
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.frizzqol.fermentercontrol";
        public const string PluginName = "FrizzQOL Fermenter Control";
        public const string PluginVersion = "0.1.0";

        internal static Plugin Instance { get; private set; }

        internal static ConfigEntry<float> Minutes;
        internal static ConfigEntry<string> TimeColor;

        private static bool _loggedMinutes;
        private static string _loggedColor;

        private Harmony _harmony;

        private void Awake()
        {
            Instance = this;
            Minutes = Config.Bind(
                "Time",
                "Minutes",
                0f,
                "Real minutes until a batch is ready. 0 keeps the normal Valheim time. Below 0 does the same.");
            TimeColor = Config.Bind(
                "Time",
                "TimeColor",
                FermentTime.FallbackColor,
                "Color of the remaining time when you look at a fermenter. A Unity color name, or #RRGGBB.");
            Minutes.SettingChanged += (_, __) => DurationApplier.ApplyAll();
            TimeColor.SettingChanged += (_, __) => NoteColor();

            _harmony = new Harmony(PluginGuid);
            try
            {
                _harmony.PatchAll();
                Logger.LogInfo($"{PluginName} {PluginVersion} loaded");
            }
            catch (System.Exception ex)
            {
                Logger.LogError("Harmony patch failed: " + ex.Message);
            }
        }

        internal static float ConfiguredMinutes()
        {
            float minutes = Minutes != null ? Minutes.Value : 0f;
            if (minutes < 0f && !_loggedMinutes)
            {
                _loggedMinutes = true;
                LogWarning("Minutes below 0 keeps the normal fermenter time.");
            }

            return minutes;
        }

        internal static string ConfiguredColor()
        {
            string color = TimeColor != null ? TimeColor.Value : FermentTime.FallbackColor;
            string safe = FermentTime.SanitizeColor(color);
            if (safe != color && _loggedColor != color)
            {
                _loggedColor = color;
                LogWarning("TimeColor '" + color + "' is not a color name or #RRGGBB. Using " + safe + ".");
            }

            return safe;
        }

        private static void NoteColor()
        {
            ConfiguredColor();
        }

        internal static void LogWarning(string message)
        {
            if (Instance != null)
            {
                Instance.Logger.LogWarning(message);
            }
        }
    }
}
