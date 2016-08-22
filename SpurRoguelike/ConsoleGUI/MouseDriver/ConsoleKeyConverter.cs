using System;
using SpurRoguelike.ConsoleGUI.WinApi;

namespace SpurRoguelike.ConsoleGUI.MouseDriver
{
    internal static class ConsoleKeyConverter
    {
        public static ConsoleKeyInfo ConvertEventToKey(KeyEvent @event)
        {
            return new ConsoleKeyInfo(@event.UnicodeChar, (ConsoleKey)@event.VirtualKeyCode, 
                @event.ControlKeyState.HasFlag(ControlKeyState.ShiftPressed),
                @event.ControlKeyState.HasFlag(ControlKeyState.LeftAltPressed) || @event.ControlKeyState.HasFlag(ControlKeyState.RightAltPressed),
                @event.ControlKeyState.HasFlag(ControlKeyState.LeftCtrlPressed) || @event.ControlKeyState.HasFlag(ControlKeyState.RightCtrlPressed));
        }
    }
}