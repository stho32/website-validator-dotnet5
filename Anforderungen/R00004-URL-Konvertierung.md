---
id: R00004
titel: "Relative-zu-Absolute URL-Konvertierung"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00004: Relative-zu-Absolute URL-Konvertierung

## Beschreibung

Relative URLs aus dem HTML-Inhalt werden anhand der Base-URL in absolute URLs umgewandelt, damit sie korrekt gecrawlt werden koennen.

## Akzeptanzkriterien

- [x] Relative URLs (z.B. `/page.html`) werden korrekt zur Base-URL aufgeloest
- [x] Bereits absolute HTTPS-URLs bleiben unveraendert
- [x] Trailing Slashes werden normalisiert
- [x] Protocol-relative URLs (`//...`) werden durchgereicht
- [x] HTTP-URLs (ohne S) werden als absolute URLs erkannt
- [x] `mailto:`, `tel:`, `javascript:`, `data:`-URLs werden korrekt ignoriert/gefiltert
- [x] Fragment-only Links (`#section`) werden korrekt gefiltert

## Implementierung

- **Hauptklasse:** `Source/WebsiteValidator.BL/Classes/RelativeToAbsoluteUrlConverter.cs` (Klasse heisst `UrlToAbsolutUrlConverter`)
- **Interface:** `Source/WebsiteValidator.BL/Interfaces/IRelativeToAbsoluteUrlConverter.cs` (heisst `IUrlToAbsolutUrlConverter`)
- **Extension Method:** `Source/WebsiteValidator.BL/ExtensionMethods/StringArrayExtensionMethods.cs`

## Bekannte Probleme

- Naming-Inkonsistenz: Dateiname, Klassenname und Interface-Name weichen voneinander ab

## Tests

- `Source/WebsiteValidator.BL.Tests/RelativeToAbsoluteUrlConverterTests.cs` (12 Tests)
