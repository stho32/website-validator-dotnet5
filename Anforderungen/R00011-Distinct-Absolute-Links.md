---
id: R00011
titel: "Distinct Absolute Links"
typ: Enhancement
status: Offen
erstellt: 2026-04-06
quelle: "GitHub Issue: #6"
---

# R00011: Distinct Absolute Links

## Beschreibung

Nach der Konvertierung relativer Links in absolute URLs sollen Duplikate entfernt werden. Wenn dieselbe URL mehrfach auf einer Seite vorkommt (z.B. in Header, Footer und Navigation), soll sie nur einmal in der Link-Liste erscheinen.

## Akzeptanzkriterien

- [ ] `ToAbsoluteUrls()` liefert nur eindeutige (distinct) URLs zurück
- [ ] Duplikate innerhalb einer Seite werden vor der Speicherung in `UrlInformation` entfernt
- [ ] Bestehende Funktionalität (Crawling, Output) bleibt unverändert
- [ ] Testabdeckung für das Distinct-Verhalten vorhanden

## Betroffene Komponenten

- **Extension Method:** `Source/WebsiteValidator.BL/ExtensionMethods/StringArrayExtensionMethods.cs` — `.Distinct().ToArray()` nach Konvertierung
- **Konsument:** `Source/WebsiteValidator.BL/Classes/Crawler.cs` (Zeile 63-65) — profitiert automatisch

## Tests

- `Source/WebsiteValidator.BL.Tests/StringArrayExtensionMethodsTests.cs` — neuer Test: Duplikate werden entfernt
