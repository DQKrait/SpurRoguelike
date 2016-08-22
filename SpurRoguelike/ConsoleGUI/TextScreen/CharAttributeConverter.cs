using System;
using SpurRoguelike.ConsoleGUI.WinApi;

namespace SpurRoguelike.ConsoleGUI.TextScreen
{
    internal static class CharAttributeConverter
    {
        public static CharAttributes MakeCharAttributes(ConsoleColor color, ConsoleColor backgroundColor)
        {
            CharAttributes attributes = 0;

            switch (color)
            {
                case ConsoleColor.DarkGray:
                    attributes |= CharAttributes.ForegroundIntensity;
                    break;
                case ConsoleColor.Gray:
                    attributes |= CharAttributes.ForegroundRed | CharAttributes.ForegroundGreen | CharAttributes.ForegroundBlue;
                    break;
                case ConsoleColor.DarkBlue:
                    attributes |= CharAttributes.ForegroundBlue;
                    break;
                case ConsoleColor.DarkGreen:
                    attributes |= CharAttributes.ForegroundGreen;
                    break;
                case ConsoleColor.DarkCyan:
                    attributes |= CharAttributes.ForegroundGreen | CharAttributes.ForegroundBlue;
                    break;
                case ConsoleColor.DarkRed:
                    attributes |= CharAttributes.ForegroundRed;
                    break;
                case ConsoleColor.DarkMagenta:
                    attributes |= CharAttributes.ForegroundRed | CharAttributes.ForegroundBlue;
                    break;
                case ConsoleColor.DarkYellow:
                    attributes |= CharAttributes.ForegroundRed | CharAttributes.ForegroundGreen;
                    break;
                case ConsoleColor.White:
                    attributes |= CharAttributes.ForegroundRed | CharAttributes.ForegroundGreen | CharAttributes.ForegroundBlue | CharAttributes.ForegroundIntensity;
                    break;
                case ConsoleColor.Blue:
                    attributes |= CharAttributes.ForegroundBlue | CharAttributes.ForegroundIntensity;
                    break;
                case ConsoleColor.Green:
                    attributes |= CharAttributes.ForegroundGreen | CharAttributes.ForegroundIntensity;
                    break;
                case ConsoleColor.Cyan:
                    attributes |= CharAttributes.ForegroundGreen | CharAttributes.ForegroundBlue | CharAttributes.ForegroundIntensity;
                    break;
                case ConsoleColor.Red:
                    attributes |= CharAttributes.ForegroundRed | CharAttributes.ForegroundIntensity;
                    break;
                case ConsoleColor.Magenta:
                    attributes |= CharAttributes.ForegroundRed | CharAttributes.ForegroundBlue | CharAttributes.ForegroundIntensity;
                    break;
                case ConsoleColor.Yellow:
                    attributes |= CharAttributes.ForegroundRed | CharAttributes.ForegroundGreen | CharAttributes.ForegroundIntensity;
                    break;
            }

            switch (backgroundColor)
            {
                case ConsoleColor.DarkGray:
                    attributes |= CharAttributes.BackgroundIntensity;
                    break;
                case ConsoleColor.Gray:
                    attributes |= CharAttributes.BackgroundRed | CharAttributes.BackgroundGreen | CharAttributes.BackgroundBlue;
                    break;
                case ConsoleColor.DarkBlue:
                    attributes |= CharAttributes.BackgroundBlue;
                    break;
                case ConsoleColor.DarkGreen:
                    attributes |= CharAttributes.BackgroundGreen;
                    break;
                case ConsoleColor.DarkCyan:
                    attributes |= CharAttributes.BackgroundGreen | CharAttributes.BackgroundBlue;
                    break;
                case ConsoleColor.DarkRed:
                    attributes |= CharAttributes.BackgroundRed;
                    break;
                case ConsoleColor.DarkMagenta:
                    attributes |= CharAttributes.BackgroundRed | CharAttributes.BackgroundBlue;
                    break;
                case ConsoleColor.DarkYellow:
                    attributes |= CharAttributes.BackgroundRed | CharAttributes.BackgroundGreen;
                    break;
                case ConsoleColor.White:
                    attributes |= CharAttributes.BackgroundRed | CharAttributes.BackgroundGreen | CharAttributes.BackgroundBlue | CharAttributes.BackgroundIntensity;
                    break;
                case ConsoleColor.Blue:
                    attributes |= CharAttributes.BackgroundBlue | CharAttributes.BackgroundIntensity;
                    break;
                case ConsoleColor.Green:
                    attributes |= CharAttributes.BackgroundGreen | CharAttributes.BackgroundIntensity;
                    break;
                case ConsoleColor.Cyan:
                    attributes |= CharAttributes.BackgroundGreen | CharAttributes.BackgroundBlue | CharAttributes.BackgroundIntensity;
                    break;
                case ConsoleColor.Red:
                    attributes |= CharAttributes.BackgroundRed | CharAttributes.BackgroundIntensity;
                    break;
                case ConsoleColor.Magenta:
                    attributes |= CharAttributes.BackgroundRed | CharAttributes.BackgroundBlue | CharAttributes.BackgroundIntensity;
                    break;
                case ConsoleColor.Yellow:
                    attributes |= CharAttributes.BackgroundRed | CharAttributes.BackgroundGreen | CharAttributes.BackgroundIntensity;
                    break;
            }

            return attributes;
        }
    }
}