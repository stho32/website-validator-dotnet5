---
id: R00010
titel: "Zusaetzliche Entry Points"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00010: Zusaetzliche Entry Points

## Beschreibung

Neben der Start-URL koennen weitere URLs aus einer Textdatei geladen werden. Dies ermoeglicht es, z.B. Sitemap-URLs oder manuell kuratierte Seiten als zusaetzliche Einstiegspunkte fuer den Crawler zu definieren.

## Akzeptanzkriterien

- [x] Textdatei mit URLs per `--additionalEntrypoints` / `--ae` CLI-Option angeben
- [x] Eine URL pro Zeile in der Datei
- [x] Leere Zeilen werden ignoriert
- [x] Duplikate mit der Start-URL werden erkannt

## Implementierung

- **Datei-Lesen:** `Source/WebsiteValidator/Program.cs` (Zeile 61)
- **Integration in Crawler:** `Source/WebsiteValidator.BL/Classes/Crawler.cs` (Zeile 29-36, Konstruktor)
