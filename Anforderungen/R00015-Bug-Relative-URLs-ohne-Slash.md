---
id: R00015
titel: "Bug-Behebung — Relative URLs ohne fuehrenden Slash werden nicht aufgeloest"
typ: Bug-Behebung
status: Abgeschlossen
erstellt: 2026-04-06
---

# R00015: Bug-Behebung — Relative URLs ohne fuehrenden Slash werden nicht aufgeloest

## Typ
Bug-Behebung

## Fehlerbeschreibung
Der Crawler findet nur 1 Seite statt vieler, wenn die Ziel-Website relative Links ohne fuehrenden Slash verwendet (z.B. `service.html` statt `/service.html`). Alle relativen Links kollabieren zur Base-URL. Entdeckt beim Crawl von https://sickingen.de/.

## Ursachenanalyse

### Root Cause
`UrlToAbsolutUrlConverter.ToAbsoluteUrl()` hatte keinen `else`-Zweig fuer relative Pfade ohne fuehrenden `/`. Der Code sprang von der `/`-Pruefung (Zeile 45) direkt zur Trailing-Slash-Bereinigung (Zeile 51), ohne die relative URL an die Base-URL anzuhaengen. Zusaetzlich normalisierte der Crawler die Base-URL nicht (Trailing-Slash), wodurch `https://sickingen.de` nicht als intern erkannt wurde bei Base-URL `https://sickingen.de/`.

### Betroffene Komponenten
- `Source/WebsiteValidator.BL/Classes/RelativeToAbsoluteUrlConverter.cs`
- `Source/WebsiteValidator.BL/Classes/Crawler.cs`

### Warum nicht durch Tests gefunden
1. Alle Converter-Tests verwendeten nur relative URLs MIT fuehrendem Slash (`/page.html`)
2. Alle Crawler-Tests verwendeten nur absolute URLs im Mock-HTML
3. Das Zusammenspiel von URL-Extraktion, Konvertierung und Crawler-Filterung war nie getestet

## Durchgefuehrte Aenderungen
- `RelativeToAbsoluteUrlConverter.cs`: `else`-Zweig fuer relative Pfade ohne `/` hinzugefuegt
- `Crawler.cs`: Base-URL mit `TrimEnd('/')` normalisiert, `IsInternalUrl()`-Methode extrahiert fuer robusten Vergleich (`url == _baseUrl || url.StartsWith(_baseUrl + "/")`)

## Test-Absicherung
- `Relative_URL_ohne_fuehrenden_Slash_wird_korrekt_aufgeloest`: Converter mit `service.html`
- `Relative_URL_ohne_fuehrenden_Slash_mit_Trailing_Slash_Base`: Converter mit Base-URL mit `/`
- `Relative_URL_in_Unterverzeichnis_ohne_fuehrenden_Slash`: Converter mit `sub/page.html`
- `CrawlEverything_folgt_relativen_Links_ohne_fuehrenden_Slash`: Crawler-Integration mit relativem Link
- `CrawlEverything_mit_Trailing_Slash_Base_URL_folgt_internen_Links`: Crawler mit Trailing-Slash Base-URL

## Erkenntnisse fuer das Projekt
Tests fuer URL-Verarbeitung muessen reale HTML-Muster abdecken — nicht nur den haeufigsten Fall (absolute URLs, relative mit `/`). Das Zusammenspiel mehrerer Komponenten (Extractor → Converter → Crawler) braucht Integrationstests mit realistischen Daten. Als Folge-Issue wurde #9 erstellt fuer `../`-Pfadnormalisierung.

## Status
Abgeschlossen — 2026-04-06
