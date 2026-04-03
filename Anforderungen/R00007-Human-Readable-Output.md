---
id: R00007
titel: "Human-Readable Output"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00007: Human-Readable Output

## Beschreibung

Alternativ zur JSON-Ausgabe kann eine menschenlesbare Konsolenausgabe aktiviert werden. Diese listet URLs nummeriert mit ihren Unterlinks auf.

## Akzeptanzkriterien

- [x] Aktivierung per `--human` / `-h` CLI-Option
- [x] Nummerierte Auflistung der URLs
- [x] Verschachtelte Darstellung der gefundenen Links pro URL

## Implementierung

- **Hauptklasse:** `Source/WebsiteValidator.BL/Classes/OutputHelpers/HumanReadableConsoleOutputHelper.cs`
- **Factory:** `Source/WebsiteValidator.BL/Classes/OutputHelpers/OutputHelperFactory.cs`

## Abhaengigkeiten

- R00002 (URL-Crawling liefert die Daten)
