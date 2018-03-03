using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GOTO.Common
{
    public class XMLSerilizable
    {
        /// <summary>
        /// 将object对象序列化成XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ObjectToXML<T>(T t, Encoding encoding)
        {
            XmlSerializer ser = new XmlSerializer(t.GetType());
            using (MemoryStream mem = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(mem, encoding))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    ser.Serialize(writer, t, ns);
                    return encoding.GetString(mem.ToArray()).Trim();
                }
            }
        }
        public static string ObjectToXML<T>(T t)
        {
            XmlSerializer ser = new XmlSerializer(t.GetType());
            using (MemoryStream mem = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(mem, Encoding.UTF8))
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    ser.Serialize(writer, t, ns);
                    return Encoding.UTF8.GetString(mem.ToArray()).Trim();
                }
            }
        }
        /// <summary>
        /// 将XML反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T XMLToObject<T>(string source, Encoding encoding)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(source)))
            {
                return (T)mySerializer.Deserialize(stream);
            }
        }
        public static T XMLToObject<T>(string source)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(source)))
            {
                return (T)mySerializer.Deserialize(stream);
            }
        }
        /// <summary>
        /// 二进制方式序列化对象
        /// </summary>
        /// <param name="testUser"></param>
        public static string Serialize<T>(T obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            return ms.ToString();
        }
        /// <summary>
        /// 二进制方式反序列化对象
        /// </summary>
        /// <returns></returns>
        public static T DeSerialize<T>(string str) where T : class
        {
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(str));
            BinaryFormatter formatter = new BinaryFormatter();
            T t = formatter.Deserialize(ms) as T;
            return t;
        }
    }
}