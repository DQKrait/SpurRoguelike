using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.ConsoleGUI.Panels
{
    internal class StatsPanel : Panel
    {
        public StatsPanel(ScreenZone zone, ITextScreen screen, Func<Player> playerProvider)
            : base(zone, screen)
        {
            this.playerProvider = playerProvider;
        }

        public override void RedrawBorder()
        {
            base.RedrawBorder();

            DrawFullWidthMessage(1, 1, new ConsoleMessage("Stats:", ConsoleColor.White));
        }

        public override void RedrawContents()
        {
            var player = playerProvider();
            if (player == null)
                return;

            var lineIndex = 0;
            DrawStatLine(ref lineIndex, "Name: " + player.Name);
            DrawStatLine(ref lineIndex, "Health: " + player.Health);
            DrawStatLine(ref lineIndex, "Attack: " + player.Attack + (player.EquippedItem == null ? "" : " + " + player.EquippedItem.AttackBonus));
            DrawStatLine(ref lineIndex, "Defence: " + player.Defence + (player.EquippedItem == null ? "" : " + " + player.EquippedItem.DefenceBonus));
            if (player.EquippedItem != null)
                DrawStatLine(ref lineIndex, "Equipped: " + player.EquippedItem.Name);
        }

        private void DrawStatLine(ref int index, string line)
        {
            DrawFullWidthMessage(1, 3 + 2 * index++, new ConsoleMessage(line, ConsoleColor.Gray));
        }

        private readonly Func<Player> playerProvider;
    }
}