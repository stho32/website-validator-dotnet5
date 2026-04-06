# WebsiteValidator

A .NET CLI tool that crawls websites and checks for HTTP errors (404, 500, etc.), broken links, and HTML validation issues.

## Features

- Crawls all internal pages starting from a given URL
- Detects HTTP errors (404, 500, etc.) across the site
- Validates HTML structure of each crawled page
- Imports additional entry points from sitemap.xml or text files
- Outputs results as JSON (console, file) or human-readable format
- Supports SSL certificate bypass for testing environments

## Installation

Requires [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet build Source/WebsiteValidator.sln
```

## Usage

```bash
# Basic crawl
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c

# Crawl with page limit
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --limit 10

# Save results to file
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c -o result.json

# Human-readable output
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --human

# Ignore SSL certificate errors
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --ignore-ssl

# Include sitemap URLs as entry points
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --sitemap

# Include URLs from a text file
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --ae additional-urls.txt

# Validate HTML of each page
dotnet run --project Source/WebsiteValidator -- -u https://example.com -c --validate-html
```

## Options

| Option | Short | Description |
|---|---|---|
| `--url` | `-u` | URL of the website to crawl (required) |
| `--crawl` | `-c` | Crawl the full site and list all links |
| `--links` | `-l` | List all links found on a single page |
| `--limit` | | Maximum number of pages to crawl |
| `--output` | `-o` | Save results to a JSON file |
| `--human` | `-h` | Human-readable output instead of JSON |
| `--ignore-ssl` | | Ignore SSL certificate errors |
| `--sitemap` | `-s` | Fetch sitemap.xml and include URLs as entry points |
| `--additionalEntrypoints` | `--ae` | Text file with additional URLs to crawl |
| `--validate-html` | `--vh` | Validate HTML structure of each page |

## Development

```bash
# Run tests
dotnet test Source/WebsiteValidator.sln

# Run tests with coverage
dotnet test Source/WebsiteValidator.sln --collect:"XPlat Code Coverage" --results-directory ./TestResults
```

## License

This project is open source.
