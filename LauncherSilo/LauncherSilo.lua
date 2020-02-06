project "LauncherSilo"
    location "./"
    kind "WindowedApp"
    language "C#"
    dotnetframework "4.6"
    icon ("RocketIcon.ico")
    files {
        "**.cs",
        "**.xaml",
        "**.config",
        "**.manifest",
        "**.ico",
        "**.png",
        "**.resx"
    }
    excludes{
        "obj/**",
        "Tests/**"
    }
    nuget {
        "ControlzEx:3.0.2.4",
        "Costura.Fody:3.3.3",
        "Extended.Wpf.Toolkit:3.6.0",
        "Fody:4.0.2",
        "gong-wpf-dragdrop:2.1.0",
        "MahApps.Metro:1.6.0",
        "MaterialDesignColors:1.2.0",
        "MaterialDesignThemes:2.6.0",
        "MaterialDesignThemes.MahApps:0.1.0"
    }
    links ("LauncherSilo.Core")
    links ("LauncherSilo.PluginSystem")
    links ("PresentationFramework")
    links ("System")
    links ("System.ComponentModel.Composition")
    links ("System.Configuration")
    links ("System.Data")
    links ("System.Drawing")
    links ("System.IO.Compression")
    links ("System.IO.Compression.FileSystem")
    links ("System.Numerics")
    links ("System.Runtime.Serialization")
    links ("System.ServiceModel")
    links ("System.Transactions")
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

    postbuildcommands { 
        "xcopy /y /r \"$(SolutionDir)output_plugins\\*\" \"$(TargetDir)plugins\\\"",
        "xcopy /y /r \"$(ProjectDir)config.xml\" \"$(TargetDir)\""
    }

    filter { "configurations:Debug" }
        defines { "DEBUG", "TRACE" }

    filter { "configurations:Release" }
        defines { "NDEBUG" }
        optimize "On"

    filter {"files:**.resx" }
        buildaction "Embed"
    filter {"files:**.ico" }
        buildaction "Resource"
    filter {"files:**.png" }
        buildaction "Resource"
