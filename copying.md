Any file in this project that doesn't state otherwise, and isn't listed in the exceptions below, is Copyright 2015 The Pokefans Authors, and is licensed under the terms of the GNU Affero General Public Licese Version 3, or (at your option) any later version ("AGPL3+"). A copy of this license can be found in legal/AGPLV3.txt.

The Pokefans Authors are:
| Full Name | Alias(es) | E-Mail |
----------------------------------
| Thomas Graf | The Libertine | kontakt@pokefans.net |
| Matthias Bogad | Delirium | delirium@hacked.xyz |

If you're a first-time commiter, add yourself to the above list. This is not just for legal reasons, but also to keep an overview of all involved nicknames.

A full list of all Pokefans authors ("contributors") can also be obtained from the VCS, e.g. via `git shortlog -sne`. Details of individual Authorship can also be obtained via the VCS, e.g. via `git blame`. 

This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITTNES FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License Version 3 for more details.

If you wish to include a file from Pokefans in your project, make sure to include all required legal info. The easisest way to do this would probably be to include a copy of this file (`copying.md`) and to leave the file's copyright header untouched.

Per-file license header guidelines:
In addition to this file, to prevent legal caveats, every souce file **must** include a header:
*pokefans-core-native*, that is, files that were created by **the pokefans-core authors**, require the following one-line header, preferrably in the first line, as a comment:
```
Copyright 20XX-20YY the pokefans-core authors. See copying.md for legal info.
```
`XXXX` is the year when the file was created, and `YYYY` is the year when the file was last edited. When editing a file, male sure the last-modification year is still correct.

*3rd-party* source files, that is, files that were taken from other open-source projects, require the following, longer header:
```
This file was ((taken|adapted)|contains (data|code)) from $PROJECT,
Copyright 1337-2013 Your Mom.
It's licensed under the terms of the 3-clause BSD license.
< any amount of lines of further legal information required by $PROJECT,
  such as a reference to a copy of the $PROJECT's README or AUTHORS file >
< if third-party files from more than the one project were used in this
  file, copy the above any number of times >
(Modifications|Other (data|code)|Everything else) Copyright 2014-2014 the openage authors.
See copying.md for further legal info.
```

Additional to the pokefans-core header, the file's original license header should be retained if in doubt. The "license" line is required only if the file is not licensed as "AGPLv3 or higher".

Authors of 3rd-party files should generally not be entered in the "pokefans-core authors" list.

All 3rd-party files *must* be included in the following list:

List of all 3rd-party files in pokefans-core:

**Currently, none.**

Notes about this file:
This file was originally created by Michael Enﬂlin for the (quite awesome) [openage project](https://github.com/SFTtech/openage/blob/master/copying.md). I (Delirium) adapted it for pokefans. Still, both of us are no lawyers. I also believe that file-level huge disclaimer blocks are a pest and should be treated as such, so I gladly took his approach. If you see any legal issues, feel free to contact me (and contact mic-e too, he'd apprecieate it).
