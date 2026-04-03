---
id: R00008
titel: "SSL-Zertifikat-Ignorierung"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00008: SSL-Zertifikat-Ignorierung

## Beschreibung

Beim Crawlen von Websites mit ungueltigem oder selbst-signiertem SSL-Zertifikat kann die Zertifikatspruefung deaktiviert werden. Dies ist nuetzlich fuer interne Entwicklungs- und Staging-Umgebungen.

## Akzeptanzkriterien

- [x] Aktivierung per `--ignore-ssl` CLI-Option
- [x] Ohne die Option: SSL-Fehler fuehren zu Exception
- [x] Mit der Option: SSL-Fehler werden ignoriert, Download funktioniert

## Implementierung

- **Hauptklasse:** `Source/WebsiteValidator.BL/Classes/DownloadAWebpage.cs` (Zeile 24-27)
- **CLI-Option:** `Source/WebsiteValidator/Program.cs` (Zeile 21)

## Tests

- `Source/WebsiteValidator.BL.Tests/DownloadAWebpageTests.cs` (3 Tests decken alle SSL-Szenarien ab)
