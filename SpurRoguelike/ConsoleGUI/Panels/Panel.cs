using System;

namespace SpurRoguelike.ConsoleGUI.Panels
{
    internal abstract class Panel
    {
        protected Panel(ScreenZone zone, ITextScreen screen)
        {
            Screen = screen;
            Zone = zone;
            ClientZone = new ScreenZone(zone.Left + 1, zone.Top + 1, zone.Width - 2, zone.Height - 2);
        }

        public virtual void RedrawBorder()
        {
            DrawHorizontalLine(Zone.Left + 1, Zone.Top, Zone.Width - 2);
            DrawHorizontalLine(Zone.Left + 1, Zone.Top + Zone.Height - 1, Zone.Width - 2);
            DrawVerticalLine(Zone.Left, Zone.Top + 1, Zone.Height - 1);
            DrawVerticalLine(Zone.Left + Zone.Width - 1, Zone.Top + 1, Zone.Height - 1);
        }

        public abstract void RedrawContents();

        public ScreenZone Zone { get; }

        public ScreenZone ClientZone { get; }

        protected void DrawFullWidthMessage(int clientLeft, int clientTop, ConsoleMessage message)
        {
            Screen.Write(ClientZone.Left + clientLeft, ClientZone.Top + clientTop, message, ClientZone.Width - clientLeft);
        }

        private void DrawHorizontalLine(int left, int top, int length)
        {
            Screen.Fill(new ScreenZone(left, top, length, 1), new ConsoleCharacter('_', ConsoleColor.White));
        }

        private void DrawVerticalLine(int left, int top, int length)
        {
            Screen.Fill(new ScreenZone(left, top, 1, length), new ConsoleCharacter('|', ConsoleColor.White));
        }

        protected readonly ITextScreen Screen;
    }
}