---
id: R00016
titel: "Bug-Behebung — Relative Pfade mit ../ und ./ werden nicht normalisiert"
typ: Bug-Behebung
status: Abgeschlossen
erstellt: 2026-04-06
---

# R00016: Bug-Behebung — Relative Pfade mit ../ und ./ werden nicht normalisiert

## Typ
Bug-Behebung

## Fehlerbeschreibung
Der `UrlToAbsolutUrlConverter` loest relative Pfade mit `../` nicht korrekt auf. Statt den uebergeordneten Pfad aufzuloesen, wird `../` einfach an die Base-URL angehaengt. `https://sickingen.de/sub/../datenschutz.html` wird zu `https://sickingen.de/../datenschutz.html` statt `https://sickingen.de/datenschutz.html`. Gleiches Problem mit `./`-Pfaden. Gemeldet in GitHub Issue #9.

## Ursachenanalyse

### Root Cause
Nach dem Zusammenbau der URL aus Base-URL und relativem Pfad wurde keine Pfadnormalisierung durchgefuehrt. Der `../`- und `./`-Anteil blieb als Literal im Pfad stehen.

### Betroffene Komponenten
- `Source/WebsiteValidator.BL/Classes/RelativeToAbsoluteUrlConverter.cs`

### Warum nicht durch Tests gefunden
Alle Converter-Tests verwendeten nur flache relative Pfade (`/page.html`, `service.html`, `sub/page.html`). Pfad-Traversierung mit `../` oder `./` wurde nie getestet — der Edge Case wurde bei R00015 erkannt und als Issue #9 ausgelagert.

## Durchgefuehrte Aenderungen
- `RelativeToAbsoluteUrlConverter.cs`: Nach dem URL-Zusammenbau wird `Uri.TryCreate()` + `GetLeftPart(UriPartial.Path)` verwendet, um `../` und `./` nativ aufzuloesen.
- `RelativeToAbsoluteUrlConverterTests.cs`: 3 neue Tests hinzugefuegt.

## Test-Absicherung
- `Relative_URL_mit_Doppelpunkt_Doppelpunkt_wird_normalisiert`: `../datenschutz.html` von `/sub` aus
- `Mehrfaches_Doppelpunkt_Doppelpunkt_wird_normalisiert`: `../../page.html` von `/a/b/c` aus
- `Punkt_Slash_wird_normalisiert`: `./page.html` von `/sub` aus

## Erkenntnisse fuer das Projekt
URL-Tests muessen neben flachen Pfaden auch Pfad-Traversierung abdecken (`../`, `./`, verschachtelte Pfade). Wenn ein Bug als bekannt erkannt und als Issue ausgelagert wird, sollte idealerweise sofort ein (ignorierter) Test geschrieben werden, damit die Luecke sichtbar bleibt.

## Status
Abgeschlossen — 2026-04-06
