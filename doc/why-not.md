# WHY NOT $stuff????

This is a list of technical descisions we made, presented in a form that hopefully makes you laugh.

## Why not php?
Because Delirium doesn't like it. That's not to say that she could not use it
_per se_, but more a thing of "if you start from scratch, the drawbacks
outweigh everything".  Specifically, php has not been chosen because

* it is not statically typed (typehints weren't a thing in 2014 and cannot substitute compiler enforced static typing)
* php ORMs suck; they scream for a preprocessor or are utterly slow
* phps syntax is a chore
* the refactoring tooling in php is inherently inferior to c#, due to dynamic typing

## Why not javascript+nodejs/python/...
Nobody in the core development team is eager to build such a huge and complex
system with these languages. node wasn't where it is now in 2014; and python
too. Also, the node ecosystem sucks and is full of script kiddies that need
packages for `Math.abs()` because the language doesn't have it.

As for python, tooling for large scale refactoring is nonexistant, and the ORMs suck.

## Why not vue/angular/react/...
If you've never heard of knockout.js, you might be surprised why not everything
is done with those shiny js frameworks. They're the new shit after all!

First, for the frontend, most of what we do is static display of content. To
have that reasonably indexed and SEO optimized, we'd need server side
rendering. While not impossible, we'd need to build the UI twice, and we're not
too fond of that.

Second, in the time we're writing the management UI, all of theses shiny
projects have had breaking API changes and major rewrites multiple times.
knockout has a steady API. call us old-fashiond, but we're like spending time
on doing new things rather than keeping up with other people's shenaningans.
And because of knockout's relative light-weightness and because the management
is not intended to be used primarily on mobile, we do not need to build a SPA
and have no intent to.

## BUT YOU WOULD GET SO MANY MORE CONTRIBUTORS IF YOU WOULD USE $SHINY-TECH
No. The problem is the complexity of the system, not the tools. It wouldn't be
much different, except that we would be much less productive.
