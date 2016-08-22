using System;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.ConsoleGUI.Panels
{
    internal class LevelViewportPanel : Panel
    {
        public LevelViewportPanel(ScreenZone zone, ITextScreen screen, Func<Level> levelProvider)
            : base(zone, screen)
        {
            this.levelProvider = levelProvider;
        }

        public override void RedrawContents()
        {
            var level = levelProvider();
            if (level == null)
                return;

            Screen.Fill(ClientZone, ConsoleCharacter.Empty);

            var leftOffset = LevelLeftOffset;
            var topOffset = LevelTopOffset;

            for (int x = Math.Max(0, -leftOffset); x < Math.Min(level.Field.Width, ClientZone.Width - leftOffset); x++)
            {
                for (int y = Math.Max(0, -topOffset); y < Math.Min(level.Field.Height, ClientZone.Height - topOffset); y++)
                {
                    var location = new Location(x, y);

                    var message = GetLocationRepresentation(location, level);
                    Screen.Put(x + leftOffset, y + topOffset, message);
                }
            }
        }

        public Location TranslateCoords(int clientLeft, int clientTop)
        {
            return new Location(clientLeft - LevelLeftOffset, clientTop - LevelTopOffset);
        }

        public void GetPlayerScreenCoords(out int left, out int top)
        {
            left = ClientZone.Left + ClientZone.Width / 2;
            top = ClientZone.Top + ClientZone.Height / 2;
        }

        public bool IsOnScreen(Entity entity)
        {
            var level = levelProvider();
            if (level != entity.Level)
                return false;

            var offset = entity.Location - level.Player.Location;

            return Math.Abs(offset.XOffset) <= ClientZone.Width / 2 && Math.Abs(offset.YOffset) <= ClientZone.Height / 2;
        }

        private int LevelLeftOffset => ClientZone.Left + ClientZone.Width / 2 - levelProvider()?.Player?.Location.X ?? 0;

        private int LevelTopOffset => ClientZone.Top + ClientZone.Height / 2 - levelProvider()?.Player?.Location.Y ?? 0;

        private static ConsoleCharacter GetLocationRepresentation(Location location, Level level)
        {
            if (level.GetEntity<Monster>(location) != null)
                return new ConsoleCharacter('@', ConsoleColor.Red);

            if (level.GetEntity<Item>(location) != null)
                return new ConsoleCharacter('$', ConsoleColor.Yellow);

            if (level.GetEntity<HealthPack>(location) != null)
                return new ConsoleCharacter('+', ConsoleColor.DarkGreen);

            if (level.GetEntity<Player>(location) != null)
                return new ConsoleCharacter('@', ConsoleColor.Yellow);

            var cellType = level.Field[location];

            switch (cellType)
            {
                case CellType.Wall:
                    return new ConsoleCharacter('#', ConsoleColor.DarkGray);
                case CellType.Trap:
                    return new ConsoleCharacter('*', ConsoleColor.DarkRed);
                case CellType.PlayerStart:
                    return new ConsoleCharacter('.', ConsoleColor.Cyan);
                case CellType.Exit:
                    return new ConsoleCharacter('!', ConsoleColor.Green);
            }

            return ConsoleCharacter.Empty;
        }

        private readonly Func<Level> levelProvider;
    }
}