# PdfMergerApp 📄

Egy egyszerű Android alkalmazás PDF fájlok összefűzéséhez, .NET MAUI segítségével.

## Funkciók

- 📂 Több PDF fájl kiválasztása a telefonról
- 🔗 PDF fájlok összefűzése egyetlen dokumentumba
- 💾 Automatikus mentés a Downloads mappába (időbélyeggel)
- ✏️ Kimeneti fájlnév testreszabása a Beállítások fülön
- ⏳ Töltésjelző animáció összefűzés közben
- 🗑️ Ideiglenes fájlok automatikus törlése

## Képernyők

| PDF összefűző | Beállítások |
|---|---|
| Fájlok listája + műveleti gombok | Kimeneti fájlnév megadása |

## Technológiák

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) – cross-platform UI framework
- [iText7](https://itextpdf.com/products/itext-core) – PDF kezelés
- [CommunityToolkit.Maui](https://github.com/CommunityToolkit/Maui) – MAUI kiegészítők

## Telepítés

1. Klónozd a repót:
   ```bash
   git clone https://github.com/felhasznalonev/PdfMergerApp.git
   ```
2. Nyisd meg Visual Studio 2022-ben
3. Állítsd be az Android SDK-t (API 21+)
4. Futtasd Android eszközön vagy emulátorban

## Követelmények

- .NET 8+
- Visual Studio 2022 (MAUI workload)
- Android 5.0 (API 21) vagy újabb

## Megjegyzések

- Az összefűzött PDF a **Downloads** mappába kerül
- A fájlnév formátuma: `[beállított_név]_[időbélyeg].pdf`
- Jelszóval védett PDF-ek megnyitásához `UnethicalReading` mód szükséges


