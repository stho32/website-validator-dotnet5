---
id: R00005
titel: "HTTP-Status-Pruefung"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00005: HTTP-Status-Pruefung

## Beschreibung

Fuer jede gecrawlte URL wird der HTTP-Statuscode erfasst und im Ergebnis gespeichert. Dies ist die Grundlage fuer die Erkennung von 404-Fehlern und anderen Problemen.

## Akzeptanzkriterien

- [x] HTTP-Statuscode wird pro URL erfasst
- [x] Bei HTTP 500 wird ein automatischer Retry durchgefuehrt
- [x] Statuscode ist im Ergebnis-Objekt (`IUrlInformation`) verfuegbar
- [ ] Gruppierung nach HTTP-Statuscode im Output
- [ ] Fehlermeldungen fuer 404 und andere Probleme generieren

## Implementierung

- **Download:** `Source/WebsiteValidator.BL/Classes/DownloadAWebpage.cs`
- **Interface:** `Source/WebsiteValidator.BL/Interfaces/IDownloadAWebpage.cs`
- **Ergebnis-Model:** `Source/WebsiteValidator.BL/Classes/Webpage.cs`
- **Ergebnis-Interface:** `Source/WebsiteValidator.BL/Interfaces/IWebpage.cs`

## Bekannte Probleme

- Kein HTTP-Timeout konfiguriert — kann bei langsamen Servern haengen
- Retry nur bei HTTP 500, nicht bei Netzwerkfehlern
- Kein User-Agent Header gesetzt

## Tests

- `Source/WebsiteValidator.BL.Tests/DownloadAWebpageTests.cs` (3 Tests, netzwerkabhaengig)
