namespace Cynamic;

using WinDbg;

internal static class Program
{
    private const string DUMP = """
        +0x000 MaximumLength    : Uint4B
        +0x004 Length           : Uint4B
        +0x008 Flags            : Uint4B
        +0x00c DebugFlags       : Uint4B
        +0x010 ConsoleHandle    : Ptr32 Void
        +0x014 ConsoleFlags     : Uint4B
        +0x018 StandardInput    : Ptr32 Void
        +0x01c StandardOutput   : Ptr32 Void
        +0x020 StandardError    : Ptr32 Void
        +0x024 CurrentDirectory : _CURDIR
        +0x030 DllPath          : _UNICODE_STRING
        +0x038 ImagePathName    : _UNICODE_STRING
        +0x040 CommandLine      : _UNICODE_STRING
        +0x048 Environment      : Ptr32 Void
        +0x04c StartingX        : Uint4B
        +0x050 StartingY        : Uint4B
        +0x054 CountX           : Uint4B
        +0x058 CountY           : Uint4B
        +0x05c CountCharsX      : Uint4B
        +0x060 CountCharsY      : Uint4B
        +0x064 FillAttribute    : Uint4B
        +0x068 WindowFlags      : Uint4B
        +0x06c ShowWindowFlags  : Uint4B
        +0x070 WindowTitle      : _UNICODE_STRING
        +0x078 DesktopInfo      : _UNICODE_STRING
        +0x080 ShellInfo        : _UNICODE_STRING
        +0x088 RuntimeData      : _UNICODE_STRING
        +0x090 CurrentDirectores : [32] _RTL_DRIVE_LETTER_CURDIR
        +0x290 EnvironmentSize  : Uint4B
        +0x294 EnvironmentVersion : Uint4B
        +0x298 PackageDependencyData : Ptr32 Void
        +0x29c ProcessGroupId   : Uint4B
        +0x2a0 LoaderThreads    : Uint4B
        +0x2a4 RedirectionDllName : _UNICODE_STRING
        +0x2ac HeapPartitionName : _UNICODE_STRING
        +0x2b4 DefaultThreadpoolCpuSetMasks : Ptr32 Uint8B
        +0x2b8 DefaultThreadpoolCpuSetMaskCount : Uint4B
        +0x2bc DefaultThreadpoolThreadMaximum : Uint4B
        +0x2c0 HeapMemoryTypeMask : Uint4B
        """;
    
    private static void Main()
    {
        _ = new DisplayTypeDump(DUMP);
    }
}
