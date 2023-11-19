namespace WinDbg;

internal record Field(string ArrayCount, string CType, string Name, int Size)
{
    internal readonly bool IsArray = string.IsNullOrEmpty(ArrayCount) is false;

    public override string ToString() => string.Format(
        "is array: {0}\narray count: {1}\ntype: {2}\nname: {3}\nsize: {4}\n",
        IsArray, ArrayCount, CType, Name, Size
    );
}
