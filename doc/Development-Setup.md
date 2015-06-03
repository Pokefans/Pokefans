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
2. If my [Pull-Request](https://github.com/mono/mono/pull/1812) isn't merged at the time of reading AND mono hasn't already incorporated the whole System.Web-Assembly, you need to apply my patch to a source tree and rebuild.
2. Download and Install monodevelop
3. If not already present, install xsp (hint: you have to compile it too if you have compiled mono yourself)
4. Clone and Build the Project
5. run xsp4

## Mac OSX
I have no Idea. But it should be similar to Linux, except you don't have a package manager and are left to yourself. If you have a working setup, edit this.

# Code-Style

There will be a document about this, for now: just use the default style from Visual Studio; that is: Spaces for everything, four spaces (in letters: F-O-U-R) for a tab; curly braces on their own line. And please do everyone a favor and xmldoc your stuff. Thanks!