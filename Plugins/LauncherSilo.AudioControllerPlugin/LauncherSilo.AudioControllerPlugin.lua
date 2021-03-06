project "LauncherSilo.AudioControllerPlugin"
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
        "Fody:4.0.2",
        "Costura.Fody:3.3.3",
        "NAudio:1.10.0"
    }
    links ("LauncherSilo.Core")
    links ("LauncherSilo.PluginSystem")
    links ("LauncherSilo.AudioControls")
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

    postbuildcommands { 
        "xcopy /y /r /e \"$(ProjectDir)\\$(OutDir)$(TargetName).*\" \"$(SolutionDir)output_plugins\\\""
    }
    
    filter { "configurations:Debug" }
        defines { "DEBUG", "TRACE" }

    filter { "configurations:Release" }
        defines { "NDEBUG" }
        optimize "On"

    filter {"files:AudioControllerPluginResource.xaml"}
        buildaction "Resource"
    filter {"files:**.ico" }
        buildaction "Resource"
    filter {"files:**.png" }
        buildaction "Resource"
