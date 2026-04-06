---
id: R00013
titel: "Sitemap-Import"
typ: Feature
quelle: "GitHub Issue: #7"
status: Offen
---

# R00013: Sitemap-Import

## Beschreibung

Neuer CLI-Parameter `--sitemap` der automatisch die `sitemap.xml` der Ziel-Website herunterlaed und alle darin enthaltenen URLs als zusaetzliche Entry Points in den Crawler einspeist.

## Akzeptanzkriterien

- [ ] Neuer CLI-Parameter `--sitemap` (Alias `-s`) als Boolean-Flag
- [ ] Wenn gesetzt, wird `<base-url>/sitemap.xml` heruntergeladen
- [ ] Alle `<loc>`-Eintraege aus der Sitemap werden als zusaetzliche Entry Points verwendet
- [ ] Sitemap-Index-Dateien (`<sitemapindex>`) werden rekursiv aufgeloest
- [ ] Fehler beim Sitemap-Download werden als Warnung ausgegeben, Crawling laeuft weiter
- [ ] Kombination mit `--ae` moeglich (beide Quellen werden zusammengefuehrt)
- [ ] Unit-Tests fuer Sitemap-XML-Parsing

## Technische Details

### Betroffene Dateien

- `Source/WebsiteValidator.BL/Interfaces/ISitemapParser.cs` (neu)
- `Source/WebsiteValidator.BL/Classes/SitemapParser.cs` (neu)
- `Source/WebsiteValidator/Program.cs` (neuer Parameter, Integration)
- `Source/WebsiteValidator.BL.Tests/SitemapParserTests.cs` (neu)

### Implementierung

1. **SitemapParser**: Klasse die XML-Sitemap parst und URLs extrahiert
   - Parst Standard-Sitemap (`<urlset><url><loc>...</loc></url></urlset>`)
   - Parst Sitemap-Index (`<sitemapindex><sitemap><loc>...</loc></sitemap></sitemapindex>`)
   - Verwendet `System.Xml.Linq` (Teil des SDK, kein zusaetzliches NuGet-Paket)

2. **CLI-Integration**: `--sitemap` Flag in Program.cs
   - Wenn gesetzt: Download von `{url}/sitemap.xml` via `IDownloadAWebpage`
   - Extrahierte URLs werden mit `--ae` URLs zusammengefuehrt
   - Alles als `additionalKnownLinks` an Crawler uebergeben

### XML-Sitemap-Format

```xml
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url><loc>https://example.com/page1</loc></url>
  <url><loc>https://example.com/page2</loc></url>
</urlset>
```

## Tests

- SitemapParser: Standard-Sitemap parsen → URLs extrahieren
- SitemapParser: Leere Sitemap → leeres Array
- SitemapParser: Sitemap-Index → Sub-Sitemap-URLs extrahieren
- SitemapParser: Ungueltige XML → leeres Array (kein Absturz)
- Integration: --sitemap Flag wird korrekt an Crawler weitergegeben
