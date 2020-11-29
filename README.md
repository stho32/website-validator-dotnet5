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

