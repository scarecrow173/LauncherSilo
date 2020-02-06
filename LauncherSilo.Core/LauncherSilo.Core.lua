project "LauncherSilo.Core"
    location "./"
    kind "SharedLib"
    language "C#"
    clr "Unsafe"
    dotnetframework "4.6"
    files {
        "**.cs",
        "NLog.config",
        "NLog.xsd"
    }

    excludes{
        "obj/**",
        "Tests/**"
    }
    nuget {
        "NLog:4.6.8"
    }
    links ("LauncherSilo.PluginSystem")
    links ("PresentationCore")
    links ("PresentationFramework")
    links ("System")
    links ("System.Configuration")
    links ("System.Core")
    links ("System.Drawing")
    links ("System.IO.Compression")
    links ("System.IO.Compression.FileSystem")
    links ("System.Runtime.Serialization")
    links ("System.ServiceModel")
    links ("System.Transactions")
    links ("System.Windows")
    links ("System.Windows.Forms")
    links ("System.Xml.Linq")
    links ("System.Data.DataSetExtensions")
    links ("Microsoft.CSharp")
    links ("System.Data")
    links ("System.Net.Http")
    links ("System.Xml")
    links ("WindowsBase")

    filter { "configurations:Debug" }
        defines { "DEBUG", "TRACE" }

    filter { "configurations:Release" }
        defines { "NDEBUG" }
        optimize "On"

    filter {"files:NLog.config"}
        buildaction "Copy"
    filter {"files:NLog.xsd"}
        buildaction "None"
