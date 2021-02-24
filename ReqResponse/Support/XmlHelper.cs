using ReqResponse.Models;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ReqResponse.Support
{
    public static class XmlHelper
    {
        public static T DeserializeObject<T>(this string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default;
            }
            try
            {
                XmlSerializer serializer = XmlSerializer.FromTypes(new[] { typeof(T) })[0];
                var stringReader = new StringReader(xml);
                using var reader = XmlReader.Create(stringReader);
                return (T)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception deserializng xml string", ex);
            }
        }

        public static string SerializeObject<T>(Type type, T toSerialize)
        {
            try
            {
                //XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

                XmlSerializer xmlSerializer = XmlSerializer.FromTypes(new[] { type })[0];

                using StringWriter textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception serializng xml string", ex);
            }
        }

        public static string CreateRequestString(string method,
                                                 string value1,
                                                 string value2)
        {
            Request request = new Request
            {
                Method = method,
                Value1 = value1,
                Value2 = value2
            };

            string result = SerializeObject(typeof(Request), request);

            return result;
        }
    }
}