Master branch: ![Build status](https://travis-ci.org/borjafdezgauna/CoderDocTools.svg?branch=master)

Develop branch: ![Build status](https://travis-ci.org/borjafdezgauna/CoderDocTools.svg?branch=develop)

# About this project
This project features some tools I wrote to make documenting code a bit easier:

- AddLicense: prepends source code (C# and C++) with the licensing text read from a text file.
- Documenter: parses triple-slash comments from source code (C# and C++) and creates a markdown wiki with the documentation.
- MarkdownToPDF: converts a markdown wiki (either hosted in Github or from a local folder) to a single PDF file. I wrote it because none of the tools I found handled linked documents. Naturally, it works with wikis output by Documenter.

More info in the wiki

## Samples

### MarkdownToPDF
- simionsoft/SimionZoo

  `MarkdownToPDF project="SimionZoo" user="simionsoft" author="Simion Soft" output-file=SimionZoo.pdf` -> [PDF](https://mega.nz/#!PqoEFaoR!uuzuw4vwNfbs69XTLnYaI3BwIp5H7a6jWrNPWTTXwRM):

- picoe/eto

   `MarkdownToPDF project="Eto" user="picoe" author="Picoe Software Solutions" output-file=Eto.pdf` -> [PDF](https://mega.nz/#!yzwSkKQR!Ay5i5z6_MjoyXZeAEAGjABTnoiWTmZiHUvsPvxhp0bc)


