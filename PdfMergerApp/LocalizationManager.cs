using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Resources;

namespace PdfMergerApp
{
    public static class LocalizationManager
    {
        private static Dictionary<string, string> _strings = new();
        private static string _currentLanguage = "hu";

        public static async Task LoadLanguageAsync(string languageCode)
        {
            _currentLanguage = languageCode;

            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync($"lang_{languageCode}.json");
                using var reader = new StreamReader(stream);
                string json = await reader.ReadToEndAsync();
                _strings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                           ?? new();
            }
            catch
            {
                // ha nem találja, üres marad
            }
        }

        public static string Get(string key)
        {
            return _strings.TryGetValue(key, out var value) ? value : key;
        }

        public static async Task SetLanguageAsync(string languageCode)
        {
            await LoadLanguageAsync(languageCode);
            Preferences.Set("language", languageCode);
        }
    }
}
