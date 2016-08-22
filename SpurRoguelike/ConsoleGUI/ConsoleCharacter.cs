using System;

namespace SpurRoguelike.ConsoleGUI
{
    internal struct ConsoleCharacter
    {
        public ConsoleCharacter(char character, ConsoleColor color, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Character = character;
            Color = color;
            BackgroundColor = backgroundColor;
        }

        public readonly char Character;
        public readonly ConsoleColor Color;
        public readonly ConsoleColor BackgroundColor;

        public static readonly ConsoleCharacter Empty = new ConsoleCharacter(' ', ConsoleColor.Black);
    }
}