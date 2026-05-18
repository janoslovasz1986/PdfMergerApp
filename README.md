# PdfMergerApp 📄
Egy Android alkalmazás PDF fájlok összefűzéséhez, .NET MAUI platformon.
Készült Claude segítségével.

## Funkciók
- 📂 Több PDF fájl kiválasztása a telefonról
- 👁️ PDF oldalak valódi előnézete (page preview)
- 🔃 Oldalak átrendezése (fel/le gombok)
- 🗑️ Egyedi oldalak törlése összefűzés előtt
- 🔗 PDF fájlok összefűzése az aktuális sorrend alapján
- 💾 Automatikus mentés a Downloads mappába (időbélyeggel)
- 🔓 Automatikus megnyitás opció elkészülés után
- ⏳ Töltésjelző animáció összefűzés közben
- 🔒 Gombok inaktiválása összefűzés közben
- 🗑️ Ideiglenes fájlok automatikus törlése

## Beállítások
- ✏️ Kimeneti fájlnév testreszabása
- 📳 Opcionális rezgés értesítés elkészüléskor
- 🔓 Kész PDF automatikus megnyitása
- 🔍 Kis / nagy oldal előnézet váltása
- 🌙 Sötét / világos mód választása

## Képernyők
| PDF összefűző | Beállítások |
|---|---|
| Fájllista + oldal preview + műveleti gombok | Fájlnév, rezgés, auto megnyitás, preview méret, téma |

## Technológiák
- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) – cross-platform UI framework (.NET 10)
- [iText7](https://itextpdf.com/products/itext-core) – PDF kezelés és összefűzés
- [CommunityToolkit.Maui](https://github.com/CommunityToolkit/Maui) – MAUI kiegészítők
- Android `PdfRenderer` – PDF oldalak valódi előnézete
- Serilog – naplózás

## Telepítés
1. Klónozd a repót:
   ```bash
   git clone https://github.com/janoslovasz1986/PdfMergerApp.git
   ```
2. Nyisd meg Visual Studio 2022-ben
3. Állítsd be az Android SDK-t (API 21+)
4. Futtasd Android eszközön vagy emulátorban

## Követelmények
- .NET 10
- Visual Studio 2022 (MAUI workload)
- Android 5.0 (API 21) vagy újabb

## Megjegyzések
- Az összefűzött PDF a **Downloads** mappába kerül
- A fájlnév formátuma: `[beállított_név]_[időbélyeg].pdf`
- Jelszóval védett PDF-ek megnyitásához `UnethicalReading` mód szükséges
- Az oldal preview és renderelés csak Android platformon érhető el
- Az oldalak sorrendje szabadon módosítható összefűzés előtt

## Licenc
MIT License
