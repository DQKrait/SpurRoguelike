using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct KeyEvent
    {
        public readonly int IsKeyDown;
        public readonly short RepeatCount;
        public readonly short VirtualKeyCode;
        public readonly short VirtualScanCode;

        [MarshalAs(UnmanagedType.U2)]
        public readonly char UnicodeChar;

        public readonly ControlKeyState ControlKeyState;
    }
}