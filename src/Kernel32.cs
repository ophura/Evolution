using System.Runtime.InteropServices;

internal static partial class Kernel32
{
    /// <summary>
    /// Retrieves a handle to the specified standard device<br/>
    /// (standard input, standard output, or standard error).
    /// </summary>
    /// <remarks>
    /// Source: <see href="https://learn.microsoft.com/en-us/windows/console/getstdhandle#parameters"/>
    /// </remarks>
    internal enum ConsoleBufferHandleType : uint
    {
        /// <summary>
        /// The standard input device.<br/>
        /// Initially, this is the console input buffer.
        /// </summary>
        InputBuffer = unchecked((uint)-10),
        
        /// <summary>
        /// The standard output device.<br/>
        /// Initially, this is the active console screen buffer.
        /// </summary>
        OutputBuffer = unchecked((uint)-11),
        
        /// <summary>
        /// The standard error device.<br/>
        /// Initially, this is the active console screen buffer.
        /// </summary>
        Error = unchecked((uint)-12)
    }
    
    /// <summary>
    /// In accordance with the <see href="https://learn.microsoft.com/en-us/windows/console/coord-str">
    /// COORD</see> <see langword="struct"/> syntax.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)] // Can be replaced with LayoutKind.Sequential
    internal struct Coordination        // (aka: implicit layout) in that case it can
    {                                   // be removed entirely along with the FieldOffset
        [FieldOffset(0)]                // members attributes if needed.
        internal short X;

        [FieldOffset(1)]
        internal short Y;
    }

    /// <summary>
    /// With respect to the <see href="https://learn.microsoft.com/en-us/windows/console/console-font-infoex">
    /// CONSOLE_FONT_INFOEX</see> <see langword="struct"/> specification.
    /// </summary>
    /// <remarks>
    /// Remarks:<br/>The <see cref="GetFontNameString"/> along with the<br/> User-defined
    /// <see langword="constructor"/> are preferential members...<br/>
    /// Feel free to exclude them if needed!
    /// <para>
    /// <see href="https://learn.microsoft.com/en-us/windows/console/console-font-infoex#remarks">
    /// Additional remarks</see>: To obtain the size of the font, pass the font index to the
    /// <see href="https://learn.microsoft.com/en-us/windows/console/getconsolefontsize">
    /// GetConsoleFontSize</see> <see langword="method"/>.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ConsoleFontInfo
    {
        // This is important, since the structure doesn't have a predefined size.
        // Using Marshal.SizeOf to resolve that at runtime.
        [FieldOffset(0)]
        private readonly uint SizeOfThis; // ULONG cbSize
        
        /// <summary>
        /// The name of the typeface (such as <b>Courier</b> or <b>Arial</b>).
        /// </summary>
        [FieldOffset(5)]
        internal unsafe fixed char FontName[32]; // WCHAR FaceName[LF_FACESIZE]

        /// <summary>
        /// A <see cref="Coordination"/> <see langword="struct"/> that contains the width
        /// and height of each character in the font, in logical units.
        /// </summary>
        /// <remarks>
        /// The <see cref="Coordination.X"/> member contains the width,
        /// while the <see cref="Coordination.Y"/> member contains the height.
        /// </remarks>
        [FieldOffset(2)]
        internal Coordination FontSize; // COORD dwFontSize

        /// <summary>
        /// The font pitch and family.
        /// </summary>
        /// <remarks>
        /// For information about the possible values for
        /// this member, see the description of the <b>tmPitchAndFamily</b> member of the
        /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-textmetrica">
        /// TEXTMETRIC</see> <see langword="struct"/>.
        /// </remarks>
        [FieldOffset(3)]
        internal uint FontFamily; // UINT FontFamily

        /// <summary>
        /// The font weight.
        /// </summary>
        /// <remarks>
        /// The weight can range from 100 to 1000, in multiples of 100.
        /// For example, the normal weight is 400, while 700 is bold.
        /// </remarks>
        [FieldOffset(4)]
        internal uint FontWeight; // UINT FontWeight

        /// <summary>
        /// The index of the font in the system's console font table.
        /// </summary>
        [FieldOffset(1)]
        internal uint FontIndex; // DWORD nFont 1

        // Can be removed... in which case, make sure to set SizeOfThis yourself before
        // referencing the instance.
        // If you do removed it, don't forget to mark SizeOfThis to be atleast internal.
        public unsafe ConsoleFontInfo()
        {
            SizeOfThis = (uint)sizeof(ConsoleFontInfo);
        }
        
        /// <summary>
        /// Optional method for ease of access and to minimize unsafe contexts elsewhere.
        /// </summary>
        /// <returns>
        /// A <see langword="string"/> that represents the <see cref="FontName"/>
        /// </returns>
        internal readonly unsafe string GetFontNameString()
        {
            fixed (char* fontName = FontName)
            {
                return Marshal.PtrToStringUTF8((nint)fontName);
            }
        }
    }

    /// <summary>
    /// Retrieves a handle to the specified standard device<br/>
    /// (<see cref="ConsoleBufferHandleType.InputBuffer"/>,
    /// <see cref="ConsoleBufferHandleType.OutputBuffer"/> <i>or</i>
    /// <see cref="ConsoleBufferHandleType.Error"/>).
    /// </summary>
    /// <returns>
    /// The specified standard device handle.
    /// </returns>
    [LibraryImport("kernel32.dll", EntryPoint = "GetStdHandle")]
    internal static partial nint GetConsoleBufferHandle(
        in ConsoleBufferHandleType bufferHandleType
    );

    /// <summary>
    /// Retrieves extended information about the current console font.
    /// </summary>
    /// <param name="consoleOutputHandle">
    /// A handle to the console screen buffer.
    /// The handle must have the <b>GENERIC_READ</b> access right.
    /// For more information, see <see href="https://learn.microsoft.com/en-us/windows/console/console-buffer-security-and-access-rights">
    /// Console Buffer Security and Access Rights.</see>
    /// </param>
    /// <param name="asMaximizedWindow">
    /// If this parameter is <see langword="true"/>, font information is retrieved for the
    /// maximum window size.
    /// <para>
    /// If this parameter is <see langword="false"/>, font
    /// information is retrieved for the current window size.
    /// </para>
    /// </param>
    /// <param name="fontInfo">
    /// A reference to a <see cref="ConsoleFontInfo"/> <see langword="struct"/> that
    /// receives the requested font information.
    /// </param>
    /// <returns>
    /// If the <see langword="method"/> succeeds,
    /// the return value is <see langword="true"/>.
    /// </returns>
    [LibraryImport("kernel32.dll", EntryPoint = "GetCurrentConsoleFontEx", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetConsoleFontInfo(
        in nint consoleOutputHandle,
        [MarshalAs(UnmanagedType.Bool)]
        in bool asMaximizedWindow,
        out ConsoleFontInfo fontInfo
    );
}
