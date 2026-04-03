# WebsiteValidator

.NET 10 CLI-Tool das Websites crawlt und auf HTTP-Fehler (404 etc.) prüft.

## Build

```bash
dotnet build Source/WebsiteValidator.sln
dotnet build Source/WebsiteValidator.sln --configuration Release
```

## Tests

```bash
dotnet test Source/WebsiteValidator.sln
dotnet test Source/WebsiteValidator.sln --configuration Release --verbosity normal
```

### Tests mit Coverage

```bash
dotnet test Source/WebsiteValidator.sln --collect:"XPlat Code Coverage" --results-directory ./TestResults
reportgenerator -reports:"./TestResults/*/coverage.cobertura.xml" -targetdir:"./TestResults/CoverageReport" -reporttypes:"TextSummary"
cat ./TestResults/CoverageReport/Summary.txt
```

## Run

```bash
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --limit 10
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c -o result.json
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --human
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --ignore-ssl
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --ae additional-urls.txt
```

## Projektstruktur

```
Source/
├── WebsiteValidator/           # Console-App (Entry Point, CLI-Optionen)
├── WebsiteValidator.BL/        # Business Logic
│   ├── Classes/                # Crawler, DownloadAWebpage, OutputHelpers, UrlConverter
│   ├── Enums/                  # ValidationMessageSeverityEnum
│   ├── ExtensionMethods/       # String[] und Webpage Extensions
│   └── Interfaces/             # Alle Interfaces (IDownloadAWebpage, IOutputHelper, etc.)
└── WebsiteValidator.BL.Tests/  # xUnit Tests (38 Tests, 96% Coverage)
```

## Technologie-Stack

- .NET 10.0 (SDK 10.0.201, gepinnt in global.json)
- HtmlAgilityPack 1.12.4 (HTML-Parsing)
- System.CommandLine 2.0.5 (CLI)
- System.Text.Json (JSON-Serialisierung, Teil des SDK)
- xUnit 2.9.3 + Moq 4.20.72 + coverlet 6.0.4 (Tests)

## CI/CD

- **CI:** `.github/workflows/build-matrix.yml` — Test + Build bei Push/PR
- **Release:** `.github/workflows/release.yml` — Automatischer Patch-Bump bei Push auf main, Manual Major/Minor via workflow_dispatch

## Konventionen

- Interfaces in `Interfaces/`, Implementierungen in `Classes/`
- Tests die Console.SetOut verwenden mit `[Collection("ConsoleOutput")]` markieren (xUnit Parallelisierung)
- Anforderungen in `Anforderungen/` mit Format `RXXXXX-Titel.md` (5-stellig mit führenden Nullen)
