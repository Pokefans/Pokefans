# Development-Setup

These steps are neccessary to go debugging:
## Windows
1. Download and Install Visual Studio (Community), 2013 or greater.
2. Download the git distribution of your trust
3. Clone the project
4. Open Visual Studio as Administrator (**important**)
5. Run the Project once, stop it immideately.
6. Go to C:\\Users\\*username*\\Documents\\IISExpress\config
7. Edit applicationhost.config
8. Change the binding for the Pokefans Project to Read something like `:port:*` (where port is ofcourse a number)
9. Now run the project again, and you're good to go.

## Linux
1. Download and Install mono
2. You need a really up to date mono. Even Arch Linux' mono (as of 2015-07-05) isn't new enough. So get something newer (at least updated after 20th of June 2015 or so).
2. Download and Install monodevelop
3. If not already present, install xsp (hint: you have to compile it too if you have compiled mono yourself)
4. Clone and Build the Project
5. run xsp4

## Mac OSX
I have no Idea. But it should be similar to Linux, except you don't have a package manager and are left to yourself. If you have a working setup, edit this.

# Code-Style
See CodeStyle.cs in this Directory.

# Installation
Please bear in mind that this is still in an early stage and an automated installer will most likely not be written as part of this project. So you have to execute some steps manually. I'd suggest running this under IIS with multiple domain bindings, as it simplifies stuff over IISExpress. XSP is fine, too.

Before running the project, make sure to make a new, empty database in your MariaDB instance. Edit the Connection String in web.config to your liking. Note that Auto-Seeding is off, so you need to manually do an Update-Database Command. Sadly the CMDlets are Windows-Only at the moment, so you'd have to invest some time to actually write a command line wrapper for it if you want to run on linux:

* [Running and Scripting Migrations from Code](http://romiller.com/2012/02/09/running-scripting-migrations-from-code/)
* [Scaffolding stuff](http://stackoverflow.com/questions/20374783/enable-entity-framework-migrations-in-mono)

Then, register a user as you normally would. Connect to the database with the database tool of your choice, and put the user in (at least) these roles (here listed with their code handle)
* `moderator`
* `superadmin`
* `role-manager`

This is enough to get you to the role manager for yourself, adding every other role within the GUI.