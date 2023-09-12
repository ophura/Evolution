namespace Uselessness;

using System.Collections.Generic;

internal static class Evolution
{
    internal static int[] GetOddIndexedElements(int[] source)
    {
        var array = new int[source.Length / 2];

        for (var i = 0; i < source.Length; i++)
        {
            if (i % 2 != 0)
            {
                array[i / 2] = source[i];
            }
        }

        return array;
    }

    internal static T[] GetOddIndexedElements<T>(T[] source)
    {
        var array = new T[source.Length / 2];

        for (var i = 0; i < source.Length; i++)
        {
            if (i % 2 == 1)
            {
                array[i / 2] = source[i];
            }
        }

        return array;
    }

    internal static List<int> GetOddIndexedElements(List<int> source)
    {
        var list = new List<int>(source.Count / 2);
        var isOddIndexed = false;

        for (var i = 0; i < source.Count; i++)
        {
            if (isOddIndexed)
            {
                list.Add(source[i]);
            }

            isOddIndexed = !isOddIndexed;
        }
        
        return list;
    }

    internal static List<T> GetOddIndexedElements<T>(List<T> source)
    {
        var list = new List<T>(source.Count / 2);

        for (var i = 1; i < source.Count; i += 2)
        {
            list.Add(source[i]);
        }

        return list;
    }

    internal static IList<T> GetOddIndexedElements<T>(IList<T> source)
    {
        var list = new List<T>(source.Count / 2);

        for (var i = 1; i < source.Count; i += 2)
        {
            list.Add(source[i]);
        }

        return list;
    }

    internal static T[] GetOddIndexedElements<T>(IReadOnlyList<T> source)
    {
        var list = new List<T>(source.Count / 2);

        for (var i = 1; i < source.Count; i += 2)
        {
            list.Add(source[i]);
        }

        return list.ToArray();
    }

    internal static IEnumerable<int> GetOddIndexedElements(IEnumerable<int> source)
    {
        var enumerator = source.GetEnumerator();
        var isOddIndexed = false;

        while (enumerator.MoveNext())
        {
            if (isOddIndexed)
            {
                yield return enumerator.Current;
            }

            isOddIndexed = !isOddIndexed;
        }

        enumerator.Dispose();
    }

    internal static IEnumerable<T> GetOddIndexedElements<T>(IEnumerable<T> source)
    {
        using var enumerator = source.GetEnumerator();

        while (enumerator.MoveNext())
        {
            enumerator.MoveNext();

            yield return enumerator.Current;
        }
    }
}
