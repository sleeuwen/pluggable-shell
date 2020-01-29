all: core plugins

core:
	dotnet publish PluggableShell -o ./publish

plugins: base64 guid json random regex

base64: core
	dotnet publish PluggableShell.Base64 -o ./publish/Plugins/Base64

guid: core
	dotnet publish PluggableShell.Guid -o ./publish/Plugins/Guid

json: core
	dotnet publish PluggableShell.Json -o ./publish/Plugins/Json

random: core
	dotnet publish PluggableShell.Random -o ./publish/Plugins/Random

regex: core
	dotnet publish PluggableShell.RegexMatch -o ./publish/Plugins/Regex
