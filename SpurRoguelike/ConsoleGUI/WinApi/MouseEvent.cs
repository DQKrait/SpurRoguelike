using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct MouseEvent
    {
        public readonly Coord MousePosition;
        public readonly MouseButtonState ButtonState;
        public readonly ControlKeyState ControlKeyState;
        public readonly MouseEventFlags EventFlags;
    }
}