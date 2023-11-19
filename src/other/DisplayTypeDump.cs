namespace WinDbg;

using System;

internal class DisplayTypeDump
{
    private readonly List<Field> fields = new();

    internal DisplayTypeDump(string dump)
    {
        var matches = GeneratedRegexes.DumpPattern().Matches(dump);
        
        for (var i = 0; i < matches.Count; ++i)
        {
            var match = matches[i];
            
            var arrayCount = match.Groups[3].Value;
            var typeString = match.Groups[4].Value;
            var name = match.Groups[2].Value;
            var offsetString = match.Groups[1].Value;

            var offset = Convert.ToInt32(offsetString, fromBase: 16);
            var field = new Field(arrayCount, typeString, name, offset);

            fields.Add(field);

            Console.WriteLine(field);
        }
    }
}
