using System;

namespace SpurRoguelike.ConsoleGUI
{
    internal struct ConsoleMessage
    {
        public ConsoleMessage(string text, ConsoleColor color, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Text = text;
            Color = color;
            BackgroundColor = backgroundColor;
        }

        public readonly string Text;
        public readonly ConsoleColor Color;
        public readonly ConsoleColor BackgroundColor;
    }
}