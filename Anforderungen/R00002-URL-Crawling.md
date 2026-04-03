---
id: R00002
titel: "URL-Crawling"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00002: URL-Crawling

## Beschreibung

Die Anwendung crawlt eine Website ausgehend von einer Start-URL. Dabei werden alle internen Links verfolgt und deren HTTP-Status erfasst. Externe Links werden erkannt, aber nicht weiter gecrawlt.

## Akzeptanzkriterien

- [x] Eine Start-URL kann per CLI-Option `--url` / `-u` angegeben werden
- [x] Mit `--crawl` / `-c` wird der Crawl-Modus aktiviert
- [x] Nur interne Links (gleiche Base-URL) werden weiter gecrawlt
- [x] Jede URL wird nur einmal besucht (Duplikat-Erkennung)
- [x] Statusreport waehrend des Crawlens auf der Konsole
- [ ] Externe URLs werden geprueft (HTTP-Status), aber deren Links nicht zurueck ins System gespeist
- [ ] CSS-, JS- und Bild-Ressourcen werden ebenfalls gecrawlt

## Implementierung

- **Hauptklasse:** `Source/WebsiteValidator.BL/Classes/Crawler.cs`
- **Entry Point:** `Source/WebsiteValidator/Program.cs` (`CrawlUrl` Methode)
- **Duplikat-Tracking:** Dictionary `_urlsWithScrapedStatus` in `Crawler.cs`

## Abhaengigkeiten

- R00003 (Link-Extraktion)
- R00004 (URL-Konvertierung)
- R00005 (HTTP-Status-Pruefung)
