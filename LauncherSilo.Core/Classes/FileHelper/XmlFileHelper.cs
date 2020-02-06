using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Diagnostics;
using Misc;

namespace FileHelper
{
    public class XmlFileHelper
    {
        static public bool SaveXmlFile<T>(string filepath, T obj, Type[] extra = null)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), extra);
                using (var streamWriter = new StreamWriter(filepath, false, Encoding.UTF8))
                {
                    serializer.Serialize(streamWriter, obj);
                    streamWriter.Flush();
                }
            }
            catch(Exception ex)
            {
                LogStatics.Debug(ex.ToString());
                return false;
            }
            return true;
        }
        static public bool SaveXmlFile<T>(Stream filestream, T obj, Type[] extra = null)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), extra);
                using (var streamWriter = new StreamWriter(filestream, Encoding.UTF8))
                {
                    serializer.Serialize(streamWriter, obj);
                    streamWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                LogStatics.Debug(ex.ToString());
                return false;
            }
            return true;
        }
        static public bool LoadXmlFile<T>(string filepath, out T obj, Type[] extra = null)
        {
            if (!System.IO.File.Exists(filepath))
            {
                obj = default(T);
                return false;
            }
            try
            {
                var xmlSettings = new System.Xml.XmlReaderSettings()
                {
                    CheckCharacters = false,
                };
                XmlSerializer serializer = new XmlSerializer(typeof(T), extra);
                using (var streamReader = new StreamReader(filepath, Encoding.UTF8))
                {
                    using (var xmlReader = System.Xml.XmlReader.Create(streamReader, xmlSettings))
                    {
                        obj = (T)serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch(Exception ex)
            {
                LogStatics.Debug(ex.ToString());
                obj = default(T);
                return false;
            }
            return true;
        }
        static public bool LoadXmlFile<T>(Stream filestream, out T obj, Type[] extra = null)
        {
            try
            {
                var xmlSettings = new System.Xml.XmlReaderSettings()
                {
                    CheckCharacters = false,
                };
                XmlSerializer serializer = new XmlSerializer(typeof(T), extra);
                using (var streamReader = new StreamReader(filestream, Encoding.UTF8))
                {
                    using (var xmlReader = System.Xml.XmlReader.Create(streamReader, xmlSettings))
                    {
                        obj = (T)serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                LogStatics.Debug(ex.ToString());
                obj = default(T);
                return false;
            }
            return true;
        }
    }
}
