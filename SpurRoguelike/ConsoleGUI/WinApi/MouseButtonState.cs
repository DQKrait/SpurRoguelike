using System;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [Flags]
    internal enum MouseButtonState
    {
        Leftmost = 0x0001,
        SecondButtonFromLeft = 0x0004,
        ThirdButtonFromLeft = 0x0008,
        FourthButtonFromLeft = 0x0010,
        Rightmost = 0x0002
    }
}