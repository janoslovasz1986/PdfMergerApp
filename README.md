# Fuse PDF 📄
Egy Android alkalmazás PDF fájlok összefűzéséhez, .NET MAUI platformon.
Készült Claude segítségével.

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
| PDF összefűző | Beállítások |
|---|---|
| Oldal preview + műveleti gombok | Fájlnév, rezgés, auto megnyitás, preview méret, téma, nyelv |
|---|---|
|Összefüzés| https://github-production-user-asset-6210df.s3.amazonaws.com/85942651/617483824-d0808db9-bdac-4072-890f-a1f09d62bc0b.jpg?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAVCODYLSA53PQK4ZA%2F20260706%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20260706T094745Z&X-Amz-Expires=300&X-Amz-Signature=70633529744b3a724b80a1395d9cf6dc4930df54035649aceac5a7a24c25fce1&X-Amz-SignedHeaders=host&response-content-type=image%2Fjpeg|

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

## Google Play áruház link
- Az applikáció letölthető play áruházból a következő linken:
  https://play.google.com/store/apps/details?id=com.lovij4ni.fusepdf