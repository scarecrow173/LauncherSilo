local config_list = {
    "Release",
    "Debug",
}
local platform_list = {
    "x64",
    "Any CPU"
}
local mask = "Plugins/**.lua"
local plugins = os.matchfiles(mask)

workspace "LauncherSilo"
    platforms(platform_list)
    configurations(config_list)
    dofile("LauncherSilo/LauncherSilo.lua")
    dofile("LauncherSilo.Core/LauncherSilo.Core.lua")
    dofile("LauncherSilo.PluginSystem/LauncherSilo.PluginSystem.lua")
    group "Plugins"
    for i = 1, #plugins do
        local plugin = plugins[i]
        dofile(plugin)
    end
