using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SmallRect
    {
        public SmallRect(short left, short top, short right, short bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public readonly short Left;
        public readonly short Top;
        public readonly short Right;
        public readonly short Bottom;
    }
}