using System;

namespace SpurRoguelike.ConsoleGUI.WinApi
{
    [Flags]
    internal enum ControlKeyState
    {
        CapslockOn = 0x0080,
        EnhancedKey = 0x0100,
        LeftAltPressed = 0x0002,
        LeftCtrlPressed = 0x0008,
        NumlockOn = 0x0020,
        RightAltPressed = 0x0001,
        RightCtrlPressed = 0x0004,
        ScrollLockOn = 0x0040,
        ShiftPressed = 0x0010
    }
}