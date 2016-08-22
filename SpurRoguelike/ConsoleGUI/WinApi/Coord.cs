using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Coord
    {
        public Coord(short x, short y)
        {
            X = x;
            Y = y;
        }

        public readonly short X;
        public readonly short Y;
    }
}