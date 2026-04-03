---
id: R00009
titel: "Crawl-Limit"
typ: Feature
status: Umgesetzt
erstellt: 2026-04-03
---

# R00009: Crawl-Limit

## Beschreibung

Die Anzahl der maximal zu crawlenden Seiten kann begrenzt werden. Dies verhindert, dass bei grossen Websites der Crawl-Vorgang zu lange dauert oder zu viele Ressourcen verbraucht.

## Akzeptanzkriterien

- [x] Begrenzung per `--limit` CLI-Option
- [x] Ohne Limit: Alle erreichbaren internen Seiten werden gecrawlt
- [x] Mit Limit: Crawling stoppt nach n Seiten

## Implementierung

- **Logik:** `Source/WebsiteValidator.BL/Classes/Crawler.cs` (Zeile 97-103)
- **CLI-Option:** `Source/WebsiteValidator/Program.cs` (Zeile 25)

## Bekannte Probleme

- Off-by-one: Limit prueft `scrapedUrls > _limit` statt `>=`, d.h. es wird eine Seite mehr gecrawlt als angegeben
