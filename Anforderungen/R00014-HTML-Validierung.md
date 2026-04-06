---
id: R00014
titel: "HTML-Validierung"
typ: Feature
quelle: "GitHub Issue: #4"
status: Offen
---

# R00014: HTML-Validierung

## Beschreibung

Lokale HTML-Validierung fuer jede gecrawlte Seite. Nutzt HtmlAgilityPack (bereits im Projekt) zur Erkennung von HTML-Parse-Fehlern. Ergebnisse werden als eigenes Feld im Output dargestellt — mit konkreten Fehlern und einem Zusammenfassungs-Flag (`IsHtmlValid`).

## Akzeptanzkriterien

- [ ] Neuer CLI-Parameter `--validate-html` (Alias `--vh`) aktiviert die HTML-Validierung
- [ ] Neue Felder in `IUrlInformation`: `IsHtmlValid` (bool) und `HtmlErrors` (string[])
- [ ] HtmlAgilityPack `ParseErrors` werden fuer jede gecrawlte Seite ausgewertet
- [ ] `IsHtmlValid` ist `true` wenn keine Parse-Fehler vorhanden, sonst `false`
- [ ] `HtmlErrors` enthaelt die konkreten Fehlermeldungen (Zeile, Spalte, Beschreibung)
- [ ] Nicht-HTML-Inhalte (Bilder, PDFs etc.) werden uebersprungen (`IsHtmlValid = true`, leeres `HtmlErrors`)
- [ ] JSON-Output enthaelt die neuen Felder automatisch
- [ ] Human-Readable-Output zeigt Validierungsergebnisse an
- [ ] Ohne `--validate-html` sind die Felder leer/default (kein Performance-Overhead)
- [ ] Unit-Tests fuer HtmlValidator

## Technische Details

### Betroffene Dateien

- `Source/WebsiteValidator.BL/Interfaces/IHtmlValidator.cs` (neu)
- `Source/WebsiteValidator.BL/Classes/HtmlValidator.cs` (neu)
- `Source/WebsiteValidator.BL/Interfaces/IUrlInformation.cs` (erweitert)
- `Source/WebsiteValidator.BL/Classes/UrlInformation.cs` (erweitert)
- `Source/WebsiteValidator.BL/Classes/Crawler.cs` (Integration)
- `Source/WebsiteValidator/Program.cs` (neuer Parameter)
- `Source/WebsiteValidator.BL/Classes/OutputHelpers/HumanReadableConsoleOutputHelper.cs` (Anzeige)
- `Source/WebsiteValidator.BL.Tests/HtmlValidatorTests.cs` (neu)

### Implementierung

1. **HtmlValidator**: Nutzt `HtmlDocument.ParseErrors` von HtmlAgilityPack
   - Gibt strukturierte Fehlermeldungen zurueck: `"Line {line}, Col {col}: {reason}"`
   - Leerer/Nicht-HTML-Content → keine Fehler

2. **IUrlInformation**: Zwei neue Properties
   - `bool IsHtmlValid` — Zusammenfassungs-Flag
   - `string[] HtmlErrors` — Konkrete Fehlerliste

3. **Crawler**: Optional HtmlValidator aufrufen (nur wenn `--validate-html` aktiv)

4. **Output**: JSON automatisch, Human-Readable explizit

## Tests

- HtmlValidator: Valides HTML → keine Fehler, IsValid = true
- HtmlValidator: HTML mit unclosed Tag → Fehler mit Zeilenangabe
- HtmlValidator: Leerer String → keine Fehler
- HtmlValidator: Nicht-HTML-Content → keine Fehler
- HtmlValidator: Mehrere Fehler → alle werden gemeldet
- Crawler-Integration: validate-html Flag steuert Validierung
