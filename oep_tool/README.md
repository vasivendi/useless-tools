# Description
This is a tool for automating OEP homework

# Setup
Compile with net7.0 or net8.0, run it directly or alias the executable in your shell

# Execution
This should be run in a shell or command prompt, Tool provides some output

## Flags
- `--wipe` removes `.oep` folder if it exists, then starts up the tool
- `--remove` removes the `.oep` folder if it exists, but exits immediately

## First time
Tool makes a new folder in the current (pwd) folder called `.oep` where the original `.sln` and `.csproj` gets copied
along with all (in the directory, but not subdirectories) `.cs` files.

Then, the csproj stored in the original folder gets modified in a way that the targeted framework value is changed to the version of the host.
This way, if we receive a csproj with a different version, it gets overridden to match the dotnet on our machine, but the zip will have the original.

## During execution
Any (not subdir) `.cs` file addition/change/rename/deletion gets synced with `.oep`

On every change, a zip file gets created with the updated `.cs` files, but with the original sln and csproj stored in `.oep`.

## Exit
Control-c on the terminal, or press escape while input is captured on the terminal

# Why?
I have a subject on Uni where we use C# in a so generic way, that it has no impact of changing versions, but have to do it every once in a while.
I hate to do it by hand, so I made this tool.
