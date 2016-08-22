using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SpurRoguelike.ConsoleGUI.WinApi;

namespace SpurRoguelike.ConsoleGUI.MouseDriver
{
    internal class ConsoleMouseDriver
    {
        public ConsoleMouseDriver(IClickHandler clickHandler)
        {
            this.clickHandler = clickHandler;
            inputHandle = GetStdHandle(InputHandle);
        }

        public ConsoleKeyInfo WaitForInput()
        {
            while (true)
            {
                int numberOfEvents;
                if (!GetNumberOfConsoleInputEvents(inputHandle, out numberOfEvents))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                if (numberOfEvents > 0)
                {
                    InputRecord record;
                    if (!ReadConsoleInput(inputHandle, out record, 1, out numberOfEvents))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    if (numberOfEvents == 0 || !Enum.IsDefined(typeof (EventType), record.EventType))
                        continue;

                    if (record.EventType == EventType.MouseEvent)
                    {
                        MouseLeft = record.MouseEvent.MousePosition.X;
                        MouseTop = record.MouseEvent.MousePosition.Y;

                        if (record.MouseEvent.ButtonState.HasFlag(MouseButtonState.Leftmost))
                            clickHandler?.HandleMouseClick(MouseLeft, MouseTop);

                        continue;
                    }

                    if (record.KeyEvent.IsKeyDown == 0)
                        continue;

                    return ConsoleKeyConverter.ConvertEventToKey(record.KeyEvent);
                }
            }
        }

        public int MouseLeft { get; private set; }

        public int MouseTop { get; private set; }

        private readonly IntPtr inputHandle;

        private readonly IClickHandler clickHandler;

        #region WinApi imports

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int handleType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetNumberOfConsoleInputEvents(IntPtr consoleHandle, out int numberOfEvents);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadConsoleInput(IntPtr consoleInputHandle, out InputRecord inputRecord, int recordCount, out int numberOfEventsRead);
        
        private const int InputHandle = -10;

        #endregion
    }
}