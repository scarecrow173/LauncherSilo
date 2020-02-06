project "LauncherSilo.FileFinderPlugin"
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
        "ControlzEx:3.0.2.4",
        "Extended.Wpf.Toolkit:3.6.0",
        "MahApps.Metro:1.6.0",
    }
    links ("LauncherSilo.Core")
    links ("LauncherSilo.PluginSystem")
    links ("System")
    links ("System.Data")
    links ("System.Xml")
    links ("Microsoft.CSharp")
    links ("System.Core")
    links ("System.Xml.Linq")
    links ("System.Data.DataSetExtensions")
    links ("System.Net.Http")
    links ("System.Xaml")
    links ("System.Windows.Forms")
    links ("WindowsBase")
    links ("PresentationCore")
    links ("PresentationFramework")

    postbuildcommands { 
        "xcopy /y /r /e \"$(ProjectDir)\\$(OutDir)$(TargetName).*\" \"$(SolutionDir)output_plugins\\\""
    }
    
    filter { "configurations:Debug" }
        defines { "DEBUG", "TRACE" }

    filter { "configurations:Release" }
        defines { "NDEBUG" }
        optimize "On"

    filter {"files:FileFinderPluginResource.xaml"}
        buildaction "Resource"
    filter {"files:**.ico" }
        buildaction "Resource"
    filter {"files:**.png" }
        buildaction "Resource"
