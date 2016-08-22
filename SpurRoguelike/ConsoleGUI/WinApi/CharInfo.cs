using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CharInfo
    {
        public CharInfo(char unicodeChar, CharAttributes attributes)
        {
            UnicodeChar = unicodeChar;
            Attributes = attributes;
        }

        [MarshalAs(UnmanagedType.U2)]
        public readonly char UnicodeChar;
        public readonly CharAttributes Attributes;
    }
}