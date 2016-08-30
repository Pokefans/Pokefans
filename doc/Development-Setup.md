# Development-Setup

These steps are neccessary to go debugging:
## Windows
1. Download and Install Visual Studio (Community), 2015 or greater.
2. Download the git distribution of your trust
3. Clone the project
4. Open Visual Studio as Administrator (**important**)
5. Install the full IIS in the Windows control panel. Make sure you install .NET with it.
5a. If you are on Windows 7, you additionally need to install the .NET Framework 4.5 into the IIS. Refer to the internet on how to do this.
6. In Visual Studio, right-click on the Pokefans-Project, select Properties and select the "Web" tab. Use "Local IIS Debugging" with a base URL of "pokefans.rocks".
7. Click "Create Virtual Directory". If there is an error, you may need to (a) adjust the project url in the Settings (b) add a binding to pokefans.rocks to your default web site in the IIS manager.
8. Configure the Web.Config from the Pokefans project to suit your needs. Make sure all specified Paths exist or strange things may happen, including, but not limited to the apocalypse and the release of all your shinies.
9. Open the NuGet Package Manager Console, and type in Update-Database to migrate the database to the newest version. Make sure "Pokefans" is your startup project and the connection string in the Web.confg is valid.
10. It may be a good idea to run `pokecli regen-search-index` to regenerate the search indexes. If you load demo data, this is neccessary for searching (and a whole lot of other features) to work properly.
11. Now run the project. Your browser should open and display your local pokefans start page.
12. Register an account
13. Use `pokecli add-privilege -u <username> -p <privilege handle>` to add yourself to the "mitarbeiter", "super-administrator", "moderator" and "role-manager" privileges.
14. Log out of the website and log in again. You can now go to mitarbeit.pokefans.rocks and use the user management system to give yourself the remaining privileges.

## Linux
1. Download and Install mono
2. You need a really up to date mono. The one from the Arch Linux Repositories or the xamarin repositories will do. Debian stables mono is too old. Ubuntu probably too.
2. Download and Install monodevelop
3. If not already present, install xsp
4. Clone and Build the Project
5. Configure the Web.Config to your likings.
6. run the dbtool to update the database.
7. It may be a good idea to run `pokecli regen-search-index` to regenerate the search indexes. If you load demo data, this is neccessary for searching (and a whole lot of other features) to work properly.
8. run the project and open pokefans.rocks in your browser.
9. Register an account
10. Use `pokecli add-privilege -u <username> -p <privilege handle>` to add yourself to the "mitarbeiter", "super-administrator", "moderator" and "role-manager" privileges
11. Log out of the website and log in again. You can now go to mitarbeit.pokefans.rocks and use the user management system to give yourself the remaining privileges.

## Mac OSX
I have no Idea. But it should be similar to Linux, except you don't have a package manager and are left to yourself. If you have a working setup, edit this.

# Code-Style
See CodeStyle.cs in this Directory.

# Installation
Please bear in mind that this is still in an early stage and an automated installer will most likely not be written as part of this project. So you have to execute some steps manually. I'd suggest running this under IIS with multiple domain bindings, as it simplifies stuff over IISExpress. XSP is fine, too.

Before running the project, make sure to make a new, empty database in your MariaDB instance. Edit the Connection String in web.config to your liking. Note that Auto-Seeding is off, so you need to manually do an Update-Database Command.

Then, register a user as you normally would. Connect to the database with the database tool of your choice, and put the user in (at least) these roles (here listed with their code handle)
* `moderator`
* `superadmin`
* `role-manager`

This is enough to get you to the role manager for yourself, adding every other role within the GUI.