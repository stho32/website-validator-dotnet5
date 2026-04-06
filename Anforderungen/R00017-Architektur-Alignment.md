# R00017 — Architektur-Alignment

## Quelle

GitHub Issue: #10

## Beschreibung

Anpassung des Projekts an die globale `dotnet-cli-tool`-Architektur-Vorlage. Umfasst Projekt-Konfiguration (Compiler-Einstellungen, Nullable, Namespaces, Formatter) und Security-Verbesserungen (CodeQL, Dependabot).

## Akzeptanzkriterien

### Projekt-Konfiguration

1. **TreatWarningsAsErrors**: Alle 3 .csproj-Dateien enthalten `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` und der Build ist fehlerfrei.
2. **Nullable**: Alle 3 .csproj-Dateien enthalten `<Nullable>enable</Nullable>` und der Build ist fehlerfrei (keine Nullable-Warnungen).
3. **File-scoped Namespaces**: Alle `.cs`-Dateien (außer `obj/`) verwenden `namespace X;` statt `namespace X { }`.
4. **Formatter-Warnungen**: Die 17 vorbestehenden Whitespace-Warnungen in Crawler.cs, HtmlAgilityBasedUrlExtractor.cs, OutputHelpers, WebpageExtensionMethods.cs sind bereinigt.

### Security

5. **CodeQL security-extended**: `.github/workflows/codeql.yml` verwendet `queries: security-extended` im Init-Schritt.
6. **Dependabot**: `.github/dependabot.yml` ist eingerichtet fuer automatische NuGet- und GitHub-Actions-Updates.

## Betroffene Dateien

### .csproj-Dateien (TreatWarningsAsErrors + Nullable)
- `Source/WebsiteValidator/WebsiteValidator.csproj`
- `Source/WebsiteValidator.BL/WebsiteValidator.BL.csproj`
- `Source/WebsiteValidator.BL.Tests/WebsiteValidator.BL.Tests.csproj`

### C#-Dateien (File-scoped Namespaces + Nullable-Fixes + Formatter)
- `Source/WebsiteValidator/Program.cs`
- `Source/WebsiteValidator.BL/Classes/Crawler.cs`
- `Source/WebsiteValidator.BL/Classes/DownloadAWebpage.cs`
- `Source/WebsiteValidator.BL/Classes/HtmlAgilityBasedUrlExtractor.cs`
- `Source/WebsiteValidator.BL/Classes/HtmlValidator.cs`
- `Source/WebsiteValidator.BL/Classes/OutputHelpers/HumanReadableConsoleOutputHelper.cs`
- `Source/WebsiteValidator.BL/Classes/OutputHelpers/JsonConsoleOutputHelper.cs`
- `Source/WebsiteValidator.BL/Classes/OutputHelpers/JsonFileOutputHelper.cs`
- `Source/WebsiteValidator.BL/Classes/OutputHelpers/OutputHelperFactory.cs`
- `Source/WebsiteValidator.BL/Classes/RelativeToAbsoluteUrlConverter.cs`
- `Source/WebsiteValidator.BL/Classes/SitemapParser.cs`
- `Source/WebsiteValidator.BL/Classes/UrlInformation.cs`
- `Source/WebsiteValidator.BL/Classes/Webpage.cs`
- `Source/WebsiteValidator.BL/Enums/ValidationMessageSeverityEnum.cs`
- `Source/WebsiteValidator.BL/ExtensionMethods/StringArrayExtensionMethods.cs`
- `Source/WebsiteValidator.BL/ExtensionMethods/WebpageExtensionMethods.cs`
- `Source/WebsiteValidator.BL/Interfaces/*.cs` (alle Interface-Dateien)
- `Source/WebsiteValidator.BL.Tests/*.cs` (alle Test-Dateien)

### CI/CD-Dateien (Security)
- `.github/workflows/codeql.yml`
- `.github/dependabot.yml` (neu)

## Teststrategie

- Alle bestehenden 66 Tests muessen weiterhin gruen sein
- Build mit `--configuration Release` muss fehlerfrei sein (TreatWarningsAsErrors)
- Keine neuen Tests noetig (rein strukturelle Aenderungen)
