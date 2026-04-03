---
id: R00006
titel: "JSON-Output"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00006: JSON-Output

## Beschreibung

Die Crawl-Ergebnisse werden als JSON ausgegeben — entweder auf die Konsole oder in eine Datei. JSON ist das Standard-Ausgabeformat und eignet sich fuer die Weiterverarbeitung mit anderen Tools (z.B. PowerShell, jq).

## Akzeptanzkriterien

- [x] JSON-Ausgabe auf Konsole (Standard, wenn kein `--output` und kein `--human`)
- [x] JSON-Ausgabe in Datei mit `--output` / `-o` Option
- [x] Pretty-Print (indented JSON)
- [x] Alle Crawl-Informationen enthalten (URL, Links, HTTP-Code, Content, Content-Groesse)

## Implementierung

- **Konsole:** `Source/WebsiteValidator.BL/Classes/OutputHelpers/JsonConsoleOutputHelper.cs`
- **Datei:** `Source/WebsiteValidator.BL/Classes/OutputHelpers/JsonFileOutputHelper.cs`
- **Factory:** `Source/WebsiteValidator.BL/Classes/OutputHelpers/OutputHelperFactory.cs`
- **Interface:** `Source/WebsiteValidator.BL/Interfaces/IOutputHelper.cs`
- **Bibliothek:** Newtonsoft.Json (soll durch System.Text.Json ersetzt werden — siehe R00001)

## Abhaengigkeiten

- R00002 (URL-Crawling liefert die Daten)
