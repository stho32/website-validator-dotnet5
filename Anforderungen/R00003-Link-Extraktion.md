---
id: R00003
titel: "Link-Extraktion"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00003: Link-Extraktion

## Beschreibung

Aus dem HTML-Inhalt einer heruntergeladenen Seite werden alle verlinkten URLs extrahiert. Dies umfasst sowohl `<a href>`-Links als auch `<img src>`-Verweise.

## Akzeptanzkriterien

- [x] Links aus `<a href="...">` werden extrahiert
- [x] Bild-URLs aus `<img src="...">` werden extrahiert
- [x] Leere oder fehlende Attribute werden ignoriert
- [ ] Links aus `<link href="...">` (CSS) werden extrahiert
- [ ] Links aus `<script src="...">` (JS) werden extrahiert

## Implementierung

- **Hauptklasse:** `Source/WebsiteValidator.BL/Classes/HtmlAgilityBasedUrlExtractor.cs`
- **Interface:** `Source/WebsiteValidator.BL/Interfaces/IUrlExtractor.cs`
- **Extension Method:** `Source/WebsiteValidator.BL/ExtensionMethods/WebpageExtensionMethods.cs`
- **Bibliothek:** HtmlAgilityPack

## Tests

- `Source/WebsiteValidator.BL.Tests/UrlExtractorTests.cs` (1 Test)
