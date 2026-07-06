# Fuse PDF 📄
Egy Android alkalmazás PDF fájlok összefűzéséhez, .NET MAUI platformon.
Készült Claude segítségével.

## Google Play áruház link
Az applikáció letölthető play áruházból a következő linken: </br>
https://play.google.com/store/apps/details?id=com.lovij4ni.fusepdf

## Funkciók
- 📂 Több PDF és kép (PNG, JPEG) fájl kiválasztása a telefonról
- 👁️ PDF oldalak valódi előnézete (page preview)
- 🔃 Oldalak átrendezése (fel/le gombok)
- 🔄 Oldalak elforgatása (bal/jobb forgatás gombokkal)
- 🗑️ Egyedi oldalak törlése összefűzés előtt
- 🔗 PDF fájlok összefűzése az aktuális sorrend alapján
- 💾 Automatikus mentés a Downloads mappába (időbélyeggel)
- 🔓 Automatikus megnyitás opció elkészülés után
- ⏳ Töltésjelző animáció összefűzés közben
- 🔒 Gombok inaktiválása összefűzés közben
- 🗑️ Ideiglenes fájlok automatikus törlése
- 🔑 Jelszóvédelem a kész PDF-hez
- 🔍 Oldalak nagyítása modal nézetben

## Beállítások
- ✏️ Kimeneti fájlnév testreszabása
- 🔓 Kész PDF automatikus megnyitása
- 🔍 Kis / nagy oldal előnézet váltása
- 🌙 Sötét / világos mód választása
- 🌍 Többnyelvű felület (Magyar, English, Deutsch, Español)
- 🖼️ Képek A4-re igazítása vagy eredeti méretben
- 🚪 Automatikus kilépés összefűzés után
- 🔑 Jelszóvédelem bekapcsolása

## Képernyők

| Oldal előnézet | Beállítások |
|---|---|
| <img src="https://github.com/user-attachments/assets/eee84f28-edae-4d27-a624-38e42bd05762" width="250"/> | <img src="https://github.com/user-attachments/assets/6b6721f6-edd8-48aa-9b28-79223494f646" width="250"/> |

| PDF és képfájlok összefűzése | Kész az összefűzés |
|---|---|
| <img src="https://github.com/user-attachments/assets/783255b6-abb2-490c-a735-27a88f17d017" width="250"/> | <img src="https://github.com/user-attachments/assets/d0808db9-bdac-4072-890f-a1f09d62bc0b" width="250"/> |

|Jelszó hozzáadása a kész PDF-hez||
|---|---|
<img src="https://github.com/user-attachments/assets/046ff7c4-ea90-4952-8821-1f5e4d834260" width="250"/> |


## Technológiák
- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) – cross-platform UI framework (.NET 10)
- [iText7](https://itextpdf.com/products/itext-core) – PDF kezelés és összefűzés
- [CommunityToolkit.Maui](https://github.com/CommunityToolkit/Maui) – MAUI kiegészítők
- Android `PdfRenderer` – PDF oldalak valódi előnézete

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
- JPEG/PNG képek automatikusan A4-re igazíthatók
- EXIF forgatás automatikusan figyelembe van véve képeknél

## Licenc
MIT License

