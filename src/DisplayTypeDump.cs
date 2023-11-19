namespace WinDbg;

using System;
using System.Text;

/// <summary>
/// represents a <see langword="class"/> for working with type dumps of <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/debugger/">WinDbg</see>
/// <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/debuggercmds/dt--display-type-">td</see> command.
/// </summary>
internal class DisplayTypeDump
{
    private readonly string[] dump;

    /// <summary>
    /// initializes a new instance of the <see cref="DisplayTypeDump"/> <see langword="class"/>.
    /// </summary>
    /// <param name="dump">
    /// a <see langword="string"/> <see cref="Array"/> representing the type <b>dump</b>.
    /// </param>
    internal DisplayTypeDump(string[] dump)
    {
        this.dump = dump;
    }

    /// <summary>
    /// initializes a new instance of the <see cref="DisplayTypeDump"/> <see langword="class"/>.
    /// </summary>
    /// <param name="dump">a <see langword="string"/> representing the type <b>dump</b>.</param>
    /// <param name="seperator">the separator used in <see cref="string.Split(string, StringSplitOptions)"/> to split the <paramref name="dump"/> <see langword="string"/>.</param>
    internal DisplayTypeDump(string dump, string seperator)
        : this(dump.Split(seperator))
    {
    }

    /// <summary>
    /// represents the 32nd character of the ASCII table.
    /// </summary>
    /// <remarks>
    /// this is the Spacebar character.
    /// </remarks>
    private const char BlankSpace = (char)ConsoleKey.Spacebar;

    /// <summary>
    /// gets the hexadecimal value from a given <b>dump</b> <see langword="string"/>.
    /// </summary>
    /// <param name="vessel">a <b>dump</b> <see langword="string"/> containing the hexadecimal value.</param>
    /// <returns>
    /// the hexadecimal value as a 64-bit singed integer.
    /// </returns>
    internal static long GetHexValue(string vessel)
    {
        const int HexBase = 16;
        var start = new Index(3);
        var end = new Index(vessel.IndexOf(BlankSpace, 6));
        var hexNumberString = vessel[start..end];
        var hexValue = Convert.ToInt64(hexNumberString, HexBase);
        return hexValue;
    }

    /// <summary>
    /// extracts the field name from the <paramref name="vessel"/> <b>dump</b> <see langword="string"/>.
    /// </summary>
    /// <param name="vessel">a <b>dump</b> <see langword="string"/> containing the field name.</param>
    /// <returns>
    /// the field name contained in the <paramref name="vessel"/>.
    /// </returns>
    internal static string GetFieldName(string vessel)
    {
        var start = new Index(7);
        var end = new Index(vessel.IndexOf(BlankSpace, start.Value));
        var fieldName = vessel[start..end];
        return fieldName;
    }

    /// <summary>
    /// gets the offset of <paramref name="target"/>.
    /// </summary>
    /// <typeparam name="T">TODO: to be documented...</typeparam>
    /// <param name="target">the <b>target</b> field whose offset is to be calculated.</param>
    /// <param name="startFrom">an optional starting field (<u>name only</u>).<br/><br/>if <see langword="null"/>, the offset is calculated from the first field.</param>
    /// <returns>
    /// the offset in multiples of <typeparamref name="T"/>s
    /// </returns>
    internal unsafe long GetOffsetOf<T>(string target, string startFrom = null!)
        where T : unmanaged
    {
        // start from the 3rd member,
        // since the first is always 0
        // and second is to be negated from.
        var index = 2;
        if (string.IsNullOrEmpty(startFrom) is false)
        {
            // if a value is given,
            // `i` becomes the index of `startFrom`
            startFrom = Array.Find(dump, field =>
            GetFieldName(field).Equals(startFrom))!;
            
            index = Array.IndexOf(dump, startFrom);
        }
        
        // previous offset - current offset == size of type.
        var currentOffset = 0L;
        var previousOffset = GetHexValue(dump[index]);
        var offsetSum = 0L;
        for (var i = index; i < dump.Length; i++)
        {
            var fieldName = GetFieldName(dump[i]);
            
            // target reached!
            if (fieldName.Equals(target)) break;

            // previous offset < current offset == true.
            currentOffset = GetHexValue(dump[i + 1]);
            
            // skip duplicates of sequentially laid-out values,
            // only considering the first occurrence.
            if (currentOffset < previousOffset) continue;

            var fieldTypeSize = currentOffset - previousOffset;
            previousOffset = currentOffset; // state-storing.

            offsetSum += fieldTypeSize;
            Console.WriteLine("{0}: {1}", fieldName, fieldTypeSize);
        }
        
        return checked(offsetSum * Convert.ToInt64(sizeof(T)));
    }
    
    internal static string GenerateCStructWithOffset<T>(string typeName, long offset)
    {
        typeName = typeName.ToUpper();
        var builder = new StringBuilder();
        builder.AppendLine($"typedef struct _{typeName} {{");
        builder.AppendLine($"    {typeof(T).Name.ToUpper()} _[{offset}];");
        builder.AppendLine($"}} {typeName}, *P{typeName}");
        return builder.ToString();
    }
}
