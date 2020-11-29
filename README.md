# website-validator
A dotnet application that crawls a website checking for http 404s and maybe more stuff later

## "big vision"

Intended usage:
```
websitevalidator -u https://www.yourdomain.whatever
```

Output: 

A big json file with a lot of information.
A part of it being the structure of the website, but also a list of validation messages from
your validators. So you can use that information for further automation.

## listing all links that are detected in a web page

Input:
```
websitevalidator -u https://www.yourdomain.whatever --links
```

Output (example):
```
   0. https://www.yourdomain.whatever/article-1234
   1. https://www.yourdomain.whatever/article-3456
   2. https://www.provenexpert.com/amazing/
   3. https://www.clickcease.com
```

## Next tasks

### basic functionality

  - [ ] convert relative urls to absolute ones
  - [ ] return the output either as human readable or json (is there a generic approach?); maybe add a --human switch for the more readable output and default to json
  - [ ] enable some basic crawling activity
    - [ ] remember the result of each url, so every url is only crawled once
    - [ ] only check external urls, but do not feed links from them back into the system. It is important that they are basically reachable but we do not want to check their pages, too)
  - [ ] also crawl resource files like linked images, css and javascript
  - [ ] add an option for a final human readable report?

### validations

  - [ ] validations should be configurable without the need for a recompilation
  - [ ] group results by http status code, create error messages for 404s and other problems
  - [ ] pages shall not contain "Error", "Warning", or anything else that looks like a php problem
  - [ ] can I have an overview of which pages are mentioned in the sitemap and which are not
  - [ ] can I have an overview of pages which are possibly disallowed by robots.txt
  - [ ] we need something that allows us to mute known validation messages that we want to ignore

