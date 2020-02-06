using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.IO.Compression;

namespace FileHelper
{
    static public class ZipFileHelper
    {
        static public bool SaveZipFile<T>(string filepath, string entryname, T obj)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    using (ZipArchive archive = ZipFile.Open(filepath, ZipArchiveMode.Update))
                    {
                        ZipArchiveEntry entry = archive.GetEntry(filepath);
                        if (entry != null)
                        {
                            XmlFileHelper.SaveXmlFile(entry.Open(), obj);
                        }
                    }
                }
                else
                {
                    string TempDir = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                    Directory.CreateDirectory(TempDir);
                    XmlFileHelper.SaveXmlFile<T>(Path.Combine(TempDir, entryname), obj);
                    ZipFile.CreateFromDirectory(TempDir, filepath, CompressionLevel.Optimal, false);
                    Directory.Delete(TempDir, true);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        static public bool LoadZipFile<T>(string filepath, string entryname, out T obj)
        {
            if (!File.Exists(filepath))
            {
                obj = default(T);
                return false;
            }
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(filepath))
                {
                    ZipArchiveEntry entry = archive.GetEntry(entryname);
                    if (entry != null)
                    {
                        XmlFileHelper.LoadXmlFile<T>(entry.Open(), out obj);
                    }
                    else
                    {
                        obj = default(T);
                    }
                }
            }
            catch
            {
                obj = default(T);
                return false;
            }
            return true;
        }

    }
}
