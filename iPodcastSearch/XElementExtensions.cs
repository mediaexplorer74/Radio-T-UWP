using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
//using static System.Net.Mime.MediaTypeNames;

namespace iPodcastSearch
{
  public static class XElementExtensions
  {
    private static readonly XNamespace itunesNamespace 
            = (XNamespace) "http://www.itunes.com/dtds/podcast-1.0.dtd";
    private static readonly XNamespace googleplayNamespace 
            = (XNamespace) "http://www.google.com/schemas/play-podcasts/1.0";
    private static readonly XNamespace atomNamespace 
            = (XNamespace) "http://www.w3.org/2005/Atom";

    public static string GetString(this XElement xElement, string name, string defaultValue = "")
    {
      XElement xelement = xElement.Element((XName) name);
      return xelement != null ? xelement.Value : defaultValue;
    }

    public static string GetStringFromItunes
    (
      this XElement xElement,
      string name,
      string defaultValue = ""
    )
    {
      XElement xelement = xElement.Element(XElementExtensions.itunesNamespace + name);
      return xelement != null ? xelement.Value : defaultValue;
    }

    public static string GetStringFromAtom
    (
      this XElement xElement,
      string name,
      string defaultValue = ""
    )
    {
      XElement xelement = xElement.Element(XElementExtensions.atomNamespace + name);
      return xelement != null ? xelement.Value : defaultValue;
    }

    public static string GetStringFromGooglePlay
    (
      this XElement xElement,
      string name,
      string defaultValue = ""
    )
    {
      XElement xelement = xElement.Element(XElementExtensions.googleplayNamespace + name);
      return xelement != null ? xelement.Value : defaultValue;
    }

    public static DateTime GetDateTime(this XElement xElement, string name)
    {
      //TEST
      //name = "title";
      
      DateTime result = DateTime.MinValue;
      //XElement xelement = xElement.Element( 
      // XElementExtensions.googleplayNamespace//XElementExtensions.googleplayNamespace 
      // + name);

      XElement xelement = xElement.Element((XName)name);

            CultureInfo culture = CultureInfo.InvariantCulture;//.CreateSpecificCulture("en-US");

            if (xelement == null)
        return result;
            string date = xelement.Value;

            string[] parts = date.Split(new char[] { ',' });

            if (parts.Length > 1)
              date = parts[1];
            date = date.TrimStart();

            //Experimental
            date = date.Replace("EST", "+2");

            string format = "dd MMM yyyy HH:mm:ss z";

            //bool r = DateTime.TryParse(xelement.Value, out result);
            result = DateTime.ParseExact(date, format, culture);

            if (result != DateTime.MinValue)
        {
            Debug.WriteLine($"Parsed {date} into {result}");
        }
        else
        {
            Debug.WriteLine($"Unable to parse {date}");
        }


        return result;
    }

    public static string ExtractAttribute(this XElement element, string attributeName)
    {
      if (element != null && element.HasAttributes)
      {
        XAttribute xattribute = element.Attribute((XName) attributeName);
        if (xattribute != null)
          return xattribute.Value;
      }
      return "";
    }

    public static string GetImageUrl(this XElement channel)
    {
      string stringFromItunes = channel.GetStringFromItunes("image");

      if (!string.IsNullOrWhiteSpace(stringFromItunes))
        return stringFromItunes;

      string stringFromGooglePlay = channel.GetStringFromGooglePlay("image");

      if (!string.IsNullOrWhiteSpace(stringFromGooglePlay))
        return stringFromGooglePlay;

      XElement xelement1 = channel.Element((XName) "image");

      if (xelement1 != null)
      {
        string imageUrl = xelement1.Element((XName) "url")?.Value;
        XElement xelement2 = xelement1.Element((XName) "title");
        if (xelement2 != null)
        {
          string str1 = xelement2.Value;
        }
        XElement xelement3 = xelement1.Element((XName) "link");
        if (xelement3 != null)
        {
          string str2 = xelement3.Value;
        }
        if (!string.IsNullOrWhiteSpace(imageUrl))
          return imageUrl;
      }
      return string.Empty;
    }
  }
}
