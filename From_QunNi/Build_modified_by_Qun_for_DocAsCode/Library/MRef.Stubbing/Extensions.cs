namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Extensions
    {
        public static IEnumerable<List<T>> BlockBuffer<T>(
            this IEnumerable<T> source,
            int bufferSize)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    var list = new List<T>(bufferSize);
                    for (int i = 0; i < bufferSize; i++)
                    {
                        if (e.MoveNext())
                        {
                            list.Add(e.Current);
                        }
                        else
                        {
                            if (list.Count > 0)
                            {
                                yield return list;
                            }
                            yield break;
                        }
                    }
                    yield return list;
                }
            }
        }
    }
}
