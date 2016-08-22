using System;
using System.Linq;

namespace SpurRoguelike.ConsoleGUI.Panels
{
    internal class HelpPanel : CenteredPanel
    {
        public HelpPanel(ITextScreen screen)
            : base(screen, BlankSize, HelpText.Max(s => s.Length), HelpText.Length)
        {
        }

        public override void RedrawContents()
        {
            Screen.Fill(ClientZone, new ConsoleCharacter(' ', ConsoleColor.Gray, ConsoleColor.Gray));
            for (int i = 0; i < HelpText.Length; i++)
                DrawFullWidthMessage(BlankSize, BlankSize + i, new ConsoleMessage(HelpText[i], ConsoleColor.Black, ConsoleColor.Gray));
        }

        private static readonly string[] HelpText =
        {
            "SPUR Roguelike - Help",
            "",
            "You suddenly appear at the middle of a large grim dungeon filled with monsters and traps. Fight your way to the light of day!",
            "",
            "Controls:",
            "",
            " Arrow keys         - move cursor to get info about everything on the level",
            "  (or mouse click)",
            "",
            "         W",
            "       A S D        - step in the corresponding direction",
            "",
            "       Q W E",
            " Alt + A   D        - attack in the corresponding direction",
            "       Z X C",
            "",
            " Space              - do nothing",
            "",
            " Esc                - quit game",
            "",
            " F1                 - display this",
            "",
            "Legend:",
            "",
            "# - wall",
            "* - trap",
            "$ - item",
            "+ - healing fruit",
            "! - exit",
            "@ - monster (or you)"
        };

        private const int BlankSize = 5;
    }
}