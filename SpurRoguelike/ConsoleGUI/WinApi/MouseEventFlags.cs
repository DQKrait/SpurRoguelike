using System;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [Flags]
    internal enum MouseEventFlags
    {
        PressedOrReleased = 0,
        Moved = 0x0001,
        DoubleClicked = 0x0002,
        Wheeled = 0x0004,
        WheeledHorizontally = 0x0008
    }
}