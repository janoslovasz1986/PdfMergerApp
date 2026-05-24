using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Resources;

namespace PdfMergerApp
{
    public static class LocalizationManager
    {
        private static string _currentLanguage = "hu";

        private static readonly Dictionary<string, Dictionary<string, string>> _strings = new()
        {
            ["hu"] = new()
            {
                ["Settings"] = "Beállítások",
                ["OutputFileName"] = "Kimeneti fájl neve",
                ["Save"] = "Mentés",
                ["Vibration"] = "Rezgés",
                ["VibrationDesc"] = "Értesítés rezgéssel ha kész a PDF",
                ["AutoOpen"] = "Automatikus megnyitás",
                ["AutoOpenDesc"] = "Kész PDF megnyitása mentés után",
                ["LargePreview"] = "Nagy preview",
                ["LargePreviewDesc"] = "Nagyobb oldal előnézet a főoldalon",
                ["DarkMode"] = "Sötét mód",
                ["DarkModeDesc"] = "Sötét témára váltás",
                ["AutoQuit"] = "Kikapcsolás",
                ["AutoQuitDesc"] = "Kilépés az appból összefűzés után",
                ["PasswordProtect"] = "Jelszóvédelem",
                ["PasswordProtectDesc"] = "A kész PDF jelszóval védve lesz",
                ["Language"] = "Nyelv",
                ["MergingInProgress"] = "PDF összefűzése folyamatban...",
                ["ErrorNoFileSelected"] = "Nincs PDF kiválasztva!",
                ["ErrorEmptyPassword"] = "A jelszó nem lehet üres!",
                ["ErrorEmptyFileName"] = "A fájlnév nem lehet üres!",
                ["PdfCreated"] = "PDF létrejött. Összesen {0} oldal.",
                ["PasswordPromptTitle"] = "Jelszóvédelem",
                ["PasswordPromptDesc"] = "Add meg a PDF jelszavát:",
                ["Cancel"] = "Mégse",
                ["Pages"] = "Oldalak",
                ["AddPdf"] = "Hozzáadás",
                ["Clear"] = "Törlés",
                ["CreatePdf"] = "PDF létrehozása",
                ["Quit"] = "Kilépés",
            },
            ["en"] = new()
            {
                ["Settings"] = "Settings",
                ["OutputFileName"] = "Output file name",
                ["Save"] = "Save",
                ["Vibration"] = "Vibration",
                ["VibrationDesc"] = "Notify with vibration when PDF is ready",
                ["AutoOpen"] = "Auto open",
                ["AutoOpenDesc"] = "Open PDF automatically after saving",
                ["LargePreview"] = "Large preview",
                ["LargePreviewDesc"] = "Larger page preview on main screen",
                ["DarkMode"] = "Dark mode",
                ["DarkModeDesc"] = "Switch to dark theme",
                ["AutoQuit"] = "Auto quit",
                ["AutoQuitDesc"] = "Exit app after merging",
                ["PasswordProtect"] = "Password protection",
                ["PasswordProtectDesc"] = "The output PDF will be password protected",
                ["Language"] = "Language",
                ["MergingInProgress"] = "Merging PDFs in progress...",
                ["ErrorNoFileSelected"] = "No PDF selected!",
                ["ErrorEmptyPassword"] = "Password cannot be empty!",
                ["ErrorEmptyFileName"] = "File name cannot be empty!",
                ["PdfCreated"] = "PDF created. Total {0} pages.",
                ["PasswordPromptTitle"] = "Password protection",
                ["PasswordPromptDesc"] = "Enter the PDF password:",
                ["Cancel"] = "Cancel",
                ["Pages"] = "Pages",
                ["AddPdf"] = "Add PDF",
                ["Clear"] = "Clear",
                ["CreatePdf"] = "Create PDF",
                ["Quit"] = "Quit",
            }
        };

        public static string Get(string key)
        {
            if (_strings.TryGetValue(_currentLanguage, out var dict) &&
                dict.TryGetValue(key, out var value))
                return value;
            return key;
        }

        public static void SetLanguage(string languageCode)
        {
            _currentLanguage = languageCode;
            Preferences.Set("language", languageCode);
        }
    }
}
