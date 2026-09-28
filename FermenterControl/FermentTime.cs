using System;
using System.Globalization;

namespace FermenterControl
{
    public static class FermentTime
    {
        public const string FallbackColor = "orange";

        public static float ResolveSeconds(float configMinutes, float vanillaSeconds)
        {
            if (configMinutes <= 0f)
            {
                return vanillaSeconds;
            }

            return configMinutes * 60f;
        }

        public static string FormatRemaining(float seconds)
        {
            if (seconds <= 0f)
            {
                return "0m";
            }

            int minutes = (int)Math.Ceiling(seconds / 60d);
            int hours = minutes / 60;
            int mins = minutes % 60;
            if (hours > 0)
            {
                return hours.ToString(CultureInfo.InvariantCulture) + "h "
                    + mins.ToString(CultureInfo.InvariantCulture) + "m";
            }

            return minutes.ToString(CultureInfo.InvariantCulture) + "m";
        }

        public static string SanitizeColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return FallbackColor;
            }

            string trimmed = color.Trim();
            if (trimmed.Length == 0 || trimmed.Length > 32)
            {
                return FallbackColor;
            }

            for (int i = 0; i < trimmed.Length; i++)
            {
                char c = trimmed[i];
                bool ok = (c >= 'a' && c <= 'z')
                    || (c >= 'A' && c <= 'Z')
                    || (c >= '0' && c <= '9')
                    || c == '#';
                if (!ok)
                {
                    return FallbackColor;
                }
            }

            return trimmed;
        }

        public static string WithRemaining(string hover, bool show, float elapsedSeconds, float durationSeconds, string color)
        {
            if (!show || string.IsNullOrEmpty(hover))
            {
                return hover ?? "";
            }

            float remain = durationSeconds - elapsedSeconds;
            if (remain <= 0f)
            {
                return hover;
            }

            string safe = SanitizeColor(color);
            return hover + "\n<color=" + safe + ">" + FormatRemaining(remain) + "</color>";
        }
    }
}
