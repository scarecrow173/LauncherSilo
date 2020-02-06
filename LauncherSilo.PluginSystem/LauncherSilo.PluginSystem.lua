project "LauncherSilo.PluginSystem"
    location "./"
    kind "SharedLib"
    language "C#"
    dotnetframework "4.6"
    files {
        "**.cs",
    }
    excludes{
        "obj/**",
        "Tests/**"
    }

    links ("Microsoft.CSharp")
    links ("System")
    links ("System.Configuration")
    links ("System.Data")
    links ("System.IO.Compression")
    links ("System.Runtime.Serialization")
    links ("System.ServiceModel")
    links ("System.Transactions")
    links ("System.Xml")

    filter { "configurations:Debug" }
        defines { "DEBUG", "TRACE" }

    filter { "configurations:Release" }
        defines { "NDEBUG" }
        optimize "On"