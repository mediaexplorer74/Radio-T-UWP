using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace iPodcastSearch
{
  public static class XmlHelper
  {
    public static string Serialize(object objectInstance)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(objectInstance.GetType());
      StringBuilder sb = new StringBuilder();
      using (StringWriter stringWriter = new StringWriter(sb))
        xmlSerializer.Serialize((TextWriter) stringWriter, objectInstance);
      return sb.ToString();
    }

    public static T Deserialize<T>(string objectData)
    {
      using (StringReader stringReader = new StringReader(objectData))
        return (T) new XmlSerializer(typeof (T)).Deserialize((TextReader) stringReader);
    }
  }
}
