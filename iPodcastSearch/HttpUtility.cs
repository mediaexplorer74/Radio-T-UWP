﻿using System;

namespace iPodcastSearch
{
  public sealed class HttpUtility
  {
    public static HttpValueCollection ParseQueryString(string query)
    {
      if (query == null)
        throw new ArgumentNullException(nameof (query));
      if (query.Length > 0 && query[0] == '?')
        query = query.Substring(1);
      return new HttpValueCollection(query, true);
    }
  }
}
