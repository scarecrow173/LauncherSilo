project "LauncherSilo.AudioControls"
    location "./"
    kind "SharedLib"
    language "C#"
    dotnetframework "4.6"
    files {
        "**.cs",
        "**.xaml",
        "**.ico",
        "**.png"
    }
    excludes{
        "obj/**",
        "Tests/**"
    }
    nuget {
    }

    links ("System")
    links ("System.Data")
    links ("System.Windows.Forms")
    links ("System.Xml")
    links ("Microsoft.CSharp")
    links ("System.Core")
    links ("System.Xml.Linq")
    links ("System.Data.DataSetExtensions")
    links ("System.Net.Http")
    links ("System.Xaml")
    links ("WindowsBase")
    links ("PresentationCore")
    links ("PresentationFramework")
    
    filter { "configurations:Debug" }
        defines { "DEBUG", "TRACE" }

    filter { "configurations:Release" }
        defines { "NDEBUG" }
        optimize "On"

    filter {"files:**.ico" }
        buildaction "Resource"
    filter {"files:**.png" }
        buildaction "Resource"
