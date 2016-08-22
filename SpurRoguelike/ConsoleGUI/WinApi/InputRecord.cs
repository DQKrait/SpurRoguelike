using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct InputRecord
    {
        [FieldOffset(0)]
        public readonly EventType EventType;

        [FieldOffset(4)]
        public readonly KeyEvent KeyEvent;

        [FieldOffset(4)]
        public readonly MouseEvent MouseEvent;
    }
}