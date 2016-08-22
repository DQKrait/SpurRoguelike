using System;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.ConsoleGUI.Panels
{
    internal class InfoPanel : Panel
    {
        public InfoPanel(ScreenZone zone, ITextScreen screen, Func<Level> levelProvider, Func<Location> locationProvider)
            : base(zone, screen)
        {
            this.levelProvider = levelProvider;
            this.locationProvider = locationProvider;
        }

        public override void RedrawContents()
        {
            var level = levelProvider();
            var location = locationProvider();
            if (level == null)
                return;

            Screen.Fill(ClientZone, ConsoleCharacter.Empty);

            DrawFullWidthMessage(1, 1, new ConsoleMessage("Info:", ConsoleColor.White));

            if (!level.Field.Contains(location))
                return;

            var lineIndex = 0;
            switch (level.Field[location])
            {
                case CellType.Wall:
                    DrawStatLine(ref lineIndex, "Wall");
                    break;
                case CellType.Trap:
                    DrawStatLine(ref lineIndex, "Trap");
                    break;
                case CellType.PlayerStart:
                case CellType.Empty:
                    var entity = level.GetEntity<Entity>(location);
                    if (entity != null)
                    {
                        DrawEntityStats(entity);
                        return;
                    }

                    DrawStatLine(ref lineIndex, "Ground");
                    break;
                case CellType.Exit:
                    DrawStatLine(ref lineIndex, "Exit");
                    break;
            }
        }

        private void DrawEntityStats(Entity entity)
        {
            var lineIndex = 0;
            if (entity is Player)
            {
                DrawStatLine(ref lineIndex, "You");
                return;
            }

            var item = entity as Item;
            if (item != null)
            {
                DrawStatLine(ref lineIndex, "Name: " + item.Name);
                DrawStatLine(ref lineIndex, "Attack bonus: " + item.AttackBonus);
                DrawStatLine(ref lineIndex, "Defence bonus: " + item.DefenceBonus);
                return;
            }

            var pawn = entity as Pawn;
            if (pawn != null)
            {
                DrawStatLine(ref lineIndex, "Name: " + pawn.Name);
                DrawStatLine(ref lineIndex, "Health: " + pawn.Health);
                DrawStatLine(ref lineIndex, "Attack: " + pawn.Attack + (pawn.EquippedItem == null ? "" : " + " + pawn.EquippedItem.AttackBonus));
                DrawStatLine(ref lineIndex, "Defence: " + pawn.Defence + (pawn.EquippedItem == null ? "" : " + " + pawn.EquippedItem.DefenceBonus));
                if (pawn.EquippedItem != null)
                    DrawStatLine(ref lineIndex, "Equipped: " + pawn.EquippedItem.Name);
                return;
            }

            DrawStatLine(ref lineIndex, entity.Name);
        }

        private void DrawStatLine(ref int index, string line)
        {
            DrawFullWidthMessage(1, 3 + 2 * index++, new ConsoleMessage(line, ConsoleColor.Gray));
        }

        private readonly Func<Level> levelProvider;
        private readonly Func<Location> locationProvider;
    }
}