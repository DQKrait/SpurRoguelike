using System;
using System.Runtime.InteropServices;
using SpurRoguelike.ConsoleGUI.WinApi;

namespace SpurRoguelike.ConsoleGUI.TextScreen
{
    internal class TextScreen : ITextScreen
    {
        public TextScreen(int width, int height)
        {
            OpenConsoleHandle();

            Console.WindowWidth = width;
            Console.WindowHeight = height;

            SetupWindow();
        }

        public TextScreen()
        {
            OpenConsoleHandle();

            ShowWindow(GetConsoleWindow(), 3);

            Console.WindowWidth = Console.LargestWindowWidth;
            Console.WindowHeight = Console.LargestWindowHeight;

            SetupWindow();
        }

        public void Clear()
        {
            Fill(new ScreenZone(0, 0, Width, Height), ConsoleCharacter.Empty);
            Render();
        }

        public void Put(int left, int top, ConsoleCharacter character)
        {
            if (left < 0 || left >= Width || top < 0 || top >= Height)
                return;

            frameBuffer[top, left] = new CharInfo(character.Character, CharAttributeConverter.MakeCharAttributes(character.Color, character.BackgroundColor));
        }

        public void Fill(ScreenZone zone, ConsoleCharacter character)
        {
            for (int i = 0; i < zone.Width; i++)
            {
                for (int j = 0; j < zone.Height; j++)
                {
                    Put(zone.Left + i, zone.Top + j, character);
                }
            }
        }

        public void Write(int left, int top, ConsoleMessage message)
        {
            Write(left, top, message, 0);
        }

        public void Write(int left, int top, ConsoleMessage message, int redrawLength)
        {
            var textToDraw = message.Text;
            if (redrawLength > 0)
                textToDraw = textToDraw.PadRight(redrawLength);

            for (int i = 0; i < textToDraw.Length; i++)
                Put(left + i, top, new ConsoleCharacter(textToDraw[i], message.Color, message.BackgroundColor));
        }

        public void Render()
        {
            var consoleRect = new SmallRect(0, 0, (short) Width, (short) Height);
            WriteConsoleOutput(consoleHandle, frameBuffer, new Coord((short) Width, (short) Height), new Coord(0, 0), ref consoleRect);
        }

        public void Render(ScreenZone zone)
        {
            var consoleRect = new SmallRect((short) zone.Left, (short) zone.Top, (short) (zone.Width + zone.Left - 1), (short) (zone.Height + zone.Top - 1));
            WriteConsoleOutput(consoleHandle, frameBuffer, new Coord((short) Width, (short) Height), new Coord((short) zone.Left, (short) zone.Top), ref consoleRect);
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        private void OpenConsoleHandle()
        {
            consoleHandle = CreateFile("CONOUT$", 0x40000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
        }

        private void SetupWindow()
        {
            Console.Clear();

            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            Width = Console.WindowWidth;
            Height = Console.WindowHeight;

            frameBuffer = new CharInfo[Height, Width];

            Clear();
        }

        private CharInfo[,] frameBuffer;
        private IntPtr consoleHandle;

        #region WinApi imports

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateFile(string fileName, int desiredAccess, int shareMode, IntPtr securityAttributes, int creationDisposition, int flagsAndAttributes, IntPtr templateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool WriteConsoleOutput(IntPtr consoleHandle, CharInfo[,] charBuffer, Coord bufferSize, Coord bufferOffset, ref SmallRect writeRegion);
        
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr windowHandle, int command);

        #endregion
    }
}