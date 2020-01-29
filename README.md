# Proof of Concept Shell with dynamically loaded plugins

A very simple shell that can be extended with plugins writen in .NET Core, using AssemblyLoadContext to load the plugins. 

Run `make all` to compile all projects and put all plugins in the correct folder to be loaded by the shell.

Then run the PluggableShell executable in the publish/ folder and have fun.
