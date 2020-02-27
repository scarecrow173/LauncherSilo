project "LauncherSilo.GraphicsInterop.WPF"
    location "./"
    kind "SharedLib"
    language "C#"
    clr "Unsafe"
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
        "Fody:4.0.2",
        "Costura.Fody:3.3.3"
    }
    links ("LauncherSilo.GraphicsInterop")
    links ("System")
    links ("System.Data")
    links ("System.Windows.Forms")
    links ("System.Xml")
    links ("System.Drawing")
    links ("Microsoft.CSharp")
    links ("System.Core")
    links ("System.Xml.Linq")
    links ("System.Data.DataSetExtensions")
    links ("System.Net.Http")
    links ("System.Xaml")
    links ("WindowsBase")
    links ("PresentationCore")
    links ("PresentationFramework")
    links ("Microsoft.Expression.Drawing")
    
    filter { "configurations:Debug" }
        defines { "DEBUG", "TRACE" }

    filter { "configurations:Release" }
        defines { "NDEBUG" }
        optimize "On"
    filter {"files:Themes/**.xaml"}
        buildaction "Page"
    filter {"files:**.ico" }
        buildaction "Resource"
    filter {"files:**.png" }
        buildaction "Resource"
