using System.IO;
//using System.Runtime.Serialization.Json;
using System.Text;

namespace iPodcastSearch
{
  public static class JsonHelper
  {
    public static string Serialize<T>(T data)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
                //RnD
        //new DataContractJsonSerializer(typeof (T)).WriteObject((Stream) memoryStream, 
        //    (object) data);
        memoryStream.Position = 0L;
        using (StreamReader streamReader = new StreamReader((Stream) memoryStream))
          return streamReader.ReadToEnd();
      }
    }

    public static T Deserialize<T>(string objString)
    {
            using 
            (  MemoryStream memoryStream 
                  = new MemoryStream(Encoding.Unicode.GetBytes(objString))
            )
            {
                return default;
                //(T) new DataContractJsonSerializer(typeof (T)).ReadObject((Stream) memoryStream);
            }
    }
  }
}
