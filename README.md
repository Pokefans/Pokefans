# Pokefans Core 3.0 "Mighty Milotic"
Das ist der Kern von Pokefans - eine Sammlung von Tools, die essentiell für die einzelnen Seiten sind. Dazu gehören z.B. ein Membershipprovider oder auch der Datenbankzugriff.

# Lizenz
![AGPLv3](https://www.gnu.org/graphics/agplv3-155x51.png)

Pokefans Core ist lizensiert unter den Bedingungen der GNU Affero General Public License version 3 (oder jeder späteren Version dieser Lizenz; terminus: AGPLv3+). Was heißt das? Jeder darf den Code herunterladen, verändern und ausführen, unter der Bedingung, dass der verändete Code klar gekennzeichnet wird und wieder unter der GNU Affero General Public License version 3 lizensiert wird. Im Unterschied zur GPL muss der Code aber auch bereit gestellt werden, wenn die Software nur als Dienst (also z.B. als Teil einer Webseite) bereit gestellt wird.

# Techniken
Und darauf bauen wir auf:

| Technik | Komponente |
| ------- | ---------- |
| MariaDB | Datenbank |
| EntityFramework 6 CodeFirst | ORM |
| C# | Programmiersprache |
| Mono | Ziel-Runtime, gemeinsam mit Vanilla-.NET und vNext |
| ASP.NET MVC5 | Webframework |
| Razor | Templatesprache |
| SAML | Login und SSO |
| NUnit | Unit-Tests |
| Nerds | Mischen das alles zusammen |

# Mitmachen
Jeder ist eingeladen sich an der Entwicklung zu beteiligen! Wenn du nicht programmieren kannst, ist das definitiv das Projekt mit dem du es lernen willst. Wirklich! Schau einfach' im IRC unter [#pokefans @ RIZON](irc://irc.rizon.net/#pokefans) vorbei oder kontaktiere uns im Board (The Libertine, Delirium, LukeSkywalker oder Birne94). Keine Sorge, es gibt immer was zu tun.

Um loslegen zu können, willst du vermutlich [Visual Studio 2013 Community](https://www.visualstudio.com/products/visual-studio-community-vs) (Windows only), [mono-develop](http://www.monodevelop.com/) oder [Visual Studio Code](http://code.visualstudio.com/) herunterladen. Einen guten Einstieg werden auch die Blogposts auf [inside.pokefans](http://inside.pokefans.net/) liefern (schon bald<sup>TM</sup>)

Wie man sieht, ist das im Moment unsere private Gitlab-Installation. Nicht jeder kann daher direkt einen Pullrequest senden. Wir werden aber bald einen Github-Mirror einrichten, wo man dann forken und Pull-Requesten kann. Einstweilen schickt bitte einfach Patches ein.

# Testing
Für dieses Backend legen wir besonderen Wert auf Unit-Tests. Jede Funktion braucht Tests! Und wer einen Fehler findet, öffnet ein Issue, behebt im idealfall den Fehler und fügt auch einen entsprechenden Test hinzu.
