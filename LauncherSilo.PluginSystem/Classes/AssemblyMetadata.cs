using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LauncherSilo.PluginSystem
{
    public class AssemblyMetadata<T> where T : class
    {
        public string DynamicLinkLibraryPath { get; set; } = string.Empty;
        public string InstanceTypeName { get { return InstanceType.FullName; } }
        public Type InstanceType { get; private set; } = null;
        public AssemblyMetadata(string DllPath, Type TargetType)
        {
            if (!typeof(T).IsAssignableFrom(TargetType))
            {
                throw new ArgumentException();
            }
            DynamicLinkLibraryPath = DllPath;
            InstanceType = TargetType;
        }

        public T CreateInstance(IPluginLogger Logger = null)
        {
            try
            {
                System.Reflection.Assembly Asm = System.Reflection.Assembly.LoadFrom(DynamicLinkLibraryPath);
                T ModuleInstance = (T)Asm.CreateInstance(InstanceTypeName);
                return ModuleInstance;
            }
            catch (Exception ex)
            {
                Logger?.Warn("new instance create failed! : {0}", ex);
                return null;
            }
        }
        static public AssemblyMetadata<T>[] SearchMetadata(string[] Paths, IPluginLogger Logger = null)
        {
            List<AssemblyMetadata<T>> MetadataResults = new List<AssemblyMetadata<T>>();
            string InterfaceName = typeof(T).FullName;
            foreach (string SearchPath in Paths)
            {
                if (!Directory.Exists(SearchPath))
                {
                    Logger?.Warn("not found search path! : {0}", SearchPath);
                    continue;
                }
                string[] Dlls = Directory.GetFiles(SearchPath, "*.dll", SearchOption.AllDirectories);
                foreach (string Dll in Dlls)
                {
                    AssemblyMetadata<T>[] Result = GetMetadatas(Dll, Logger);
                    if (Result != null)
                    {
                        MetadataResults.AddRange(Result);
                    }
                }
            }
            return MetadataResults.ToArray();
        }
        static public AssemblyMetadata<T>[] GetMetadatas(string DllFilePath, IPluginLogger Logger = null)
        {
            if (!File.Exists(DllFilePath))
            {
                Logger?.Warn("not found DLL path! : {0}", DllFilePath);
                return null;
            }
            List<AssemblyMetadata<T>> MetadataResults = new List<AssemblyMetadata<T>>();
            try
            {
                System.Reflection.Assembly Asm = System.Reflection.Assembly.LoadFrom(DllFilePath);
                MetadataResults.AddRange(GetMetadatas(Asm, Logger));
            }
            catch (Exception ex)
            {
                Logger?.Warn("has exception! : {0}", ex);
            }
            return MetadataResults.ToArray();
        }
        static public AssemblyMetadata<T>[] GetMetadatas(System.Reflection.Assembly MetadataAssembly, IPluginLogger Logger = null)
        {

            List<AssemblyMetadata<T>> MetadataResults = new List<AssemblyMetadata<T>>();
            string InterfaceName = typeof(T).FullName;
            try
            {
                foreach (Type type in MetadataAssembly.GetTypes())
                {
                    if (!type.IsClass || !type.IsPublic || type.IsAbstract || type.GetInterface(InterfaceName) == null)
                    {
                        continue;
                    }
                    Logger?.Debug("found plugin : {0} in {1}", type.FullName, MetadataAssembly.Location);
                    MetadataResults.Add(new AssemblyMetadata<T>(MetadataAssembly.Location, type));
                }
            }
            catch (Exception ex)
            {
                Logger?.Warn("has exception! : {0}", ex);
            }
            return MetadataResults.ToArray();
        }
    }
}
