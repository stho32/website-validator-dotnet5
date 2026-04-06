---
id: R00012
titel: "CodeQL-Analyse"
typ: Chore
quelle: "GitHub Issue: #5"
status: Offen
---

# R00012: CodeQL-Analyse

## Beschreibung

GitHub CodeQL als Security-Analyse-Workflow einrichten. CodeQL erkennt automatisch Sicherheitsluecken und Code-Qualitaetsprobleme im C#-Code.

## Akzeptanzkriterien

- [x] CodeQL GitHub Actions Workflow vorhanden unter `.github/workflows/codeql.yml`
- [x] Analyse laeuft bei Push auf `main` und bei Pull Requests
- [x] Analyse laeuft zusaetzlich woechentlich (Schedule) fuer neue CVE-Erkennung
- [x] C#/.NET wird als Sprache konfiguriert
- [x] Workflow ist konsistent mit bestehenden Workflows (gleiche .NET-Version, checkout-Action)

## Technische Details

### Betroffene Dateien

- `.github/workflows/codeql.yml` (neu)

### Implementierung

Standard GitHub CodeQL-Workflow mit:
- `github/codeql-action/init@v3`
- `github/codeql-action/autobuild@v3`
- `github/codeql-action/analyze@v3`
- Sprache: `csharp`
- Trigger: push, pull_request, schedule (woechentlich)

## Tests

Kein Produktionscode betroffen — Verifikation erfolgt durch erfolgreichen Workflow-Lauf auf GitHub.
