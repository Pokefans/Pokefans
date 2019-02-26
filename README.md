English version below.

# Pokefans Core 3.0 "Mighty Milotic"
Das ist der neue Kern von Pokefans - eine der größten Pokemon-Fanseiten im DACH-Raum.

# Lizenz
![AGPLv3](https://www.gnu.org/graphics/agplv3-155x51.png)

Pokefans Core ist lizenziert unter den Bedingungen der GNU Affero General Public License Version 3 (oder jeder späteren Version dieser Lizenz; Terminus: AGPLv3+). Was heißt das? Jeder darf den Code herunterladen, verändern und ausführen, unter der Bedingung, dass der verändete Code klar gekennzeichnet wird und wieder unter der GNU Affero General Public License Version 3 lizenziert wird. Im Unterschied zur GPL muss der Code aber auch bereit gestellt werden, wenn die Software nur als Dienst (also z.B. als Teil einer Webseite) bereit gestellt wird.

# Mitmachen
Jeder ist eingeladen, sich an der Entwicklung zu beteiligen! Wenn du nicht programmieren kannst, ist das definitiv das Projekt mit dem du es lernen willst. Wirklich! Schau einfach im IRC unter [#pokefans @ RIZON](irc://irc.rizon.net/#pokefans) vorbei oder kontaktiere uns im Board (The Libertine, Delirium, LukeSkywalker oder Birne94). Keine Sorge, es gibt immer was zu tun.

Um loslegen zu können, willst du vermutlich [Visual Studio Community](https://www.visualstudio.com/products/visual-studio-community-vs) (Windows / macOS) oder [mono-develop](http://www.monodevelop.com/)  herunterladen. Einen guten Einstieg werden auch die Blogposts auf [inside.pokefans](http://inside.pokefans.net/) liefern (schon bald<sup>TM</sup>)


# Pokefans Core 3.0 "Mighty Milotic"
This is the new Core of Pokefans - one of europe's leading Pokemon communities. It's licensed under the terms of the AGPLv3 license, or, at your choosing, any later version of this license. You can find the details in contributing.md ;-)

Contributing code is easy - just think of an awesome feature and open an issue to find out if it has chances to get merged. There's a certain limit on what we accept as a feature, but as long as it has to do with pokemon, you're pretty much fine. To get started, download [Visual Studio Community](https://www.visualstudio.com/products/visual-studio-community-vs) (Windows/macOS) or [mono-develop](http://www.monodevelop.com/). To get a general idea of what we're doing here, read our blog [inside.pokefans](http://inside.pokefans.net/) (german only).

## Building the solution
Since January 2019, `node` and `npm` are build dependencies (because node.js is like cancer and creeps literally everywhere).

Everything can be neatly summarized like this:

```
git clone --recursive git@github.com:Pokefans/Pokefans
cd Pokefans/Pokefans
npm install -D
msbuild
```

Our SASS compilation is triggered as a postcompile step through msbuild, but can also be run manually with `npm run dist`. *Attention*: without bootstrap as a submodule build *will* fail, because we re-use bootstrap's postcss config.

To run the solution, use your favourite web server; on Windows we recommend IIS (make sure the site you're binding binds to `pokefans.rocks` and `*.pokefans.rocks`), everywhere else xsp4 is fine and doesn't need any configuration.
