using System;
using SpurRoguelike.ConsoleGUI.MouseDriver;
using SpurRoguelike.ConsoleGUI.Panels;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.ConsoleGUI
{
    internal class ConsoleGui : IClickHandler
    {
        public ConsoleGui(ITextScreen screen)
        {
            this.screen = screen;

            mouseDriver = new ConsoleMouseDriver(this);

            Console.CursorSize = 100;
            
            LayoutScreen();
            DrawInterface();
        }

        public bool IsOnScreen(Entity entity)
        {
            if (entity == null)
                return true;

            return levelViewportPanel.IsOnScreen(entity);
        }

        public void RenderLevel(Level level)
        {
            if (this.level != level)
            {
                this.level = level;
                InitCursorPosition();
                screen.Fill(levelViewportPanel.ClientZone, ConsoleCharacter.Empty);
            }

            levelViewportPanel.RedrawContents();
            statsPanel.RedrawContents();
            infoPanel.RedrawContents();
            messagesPanel.RedrawContents();

            screen.Render();
        }

        public Turn AskPlayerInput()
        {
            while (true)
            {
                var key = mouseDriver.WaitForInput();

                Console.CancelKeyPress += (sender, args) =>
                    args.Cancel = true;

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        return null;
                    case ConsoleKey.LeftArrow:
                        MoveCursor(-1, 0);
                        break;
                    case ConsoleKey.UpArrow:
                        MoveCursor(0, -1);
                        break;
                    case ConsoleKey.RightArrow:
                        MoveCursor(1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        MoveCursor(0, 1);
                        break;
                    case ConsoleKey.A:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.West);
                        return Turn.Step(StepDirection.West);
                    case ConsoleKey.C:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.SouthEast);
                        break;
                    case ConsoleKey.D:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.East);
                        return Turn.Step(StepDirection.East);
                    case ConsoleKey.E:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.NorthEast);
                        break;
                    case ConsoleKey.Q:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.NorthWest);
                        break;
                    case ConsoleKey.S:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.South);
                        return Turn.Step(StepDirection.South);
                    case ConsoleKey.W:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.North);
                        return Turn.Step(StepDirection.North);
                    case ConsoleKey.X:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.South);
                        break;
                    case ConsoleKey.Z:
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Alt))
                            return Turn.Attack(AttackDirection.SouthWest);
                        break;
                    case ConsoleKey.Spacebar:
                        return Turn.None;
                    case ConsoleKey.F1:
                        DisplayHelp();
                        break;
                }
            }
        }

        public void EndGame(bool isCompleted)
        {
            var gameOverPanel = new MessagePanel(screen,
                isCompleted ? new ConsoleMessage("WELCOME TO FREEDOM", ConsoleColor.DarkGreen, ConsoleColor.Gray) :
                              new ConsoleMessage("GAME OVER", ConsoleColor.DarkRed, ConsoleColor.Gray));

            levelViewportPanel.RedrawContents();
            statsPanel.RedrawContents();
            messagesPanel.RedrawContents();

            gameOverPanel.RedrawContents();

            screen.Render();

            Console.ReadKey();
            Exit();
        }

        public void Exit()
        {
            screen.Clear();
            Environment.Exit(0);
        }

        public void DisplayMessage(ConsoleMessage message)
        {
            messagesPanel.AddMessage(message);
        }

        public void HandleMouseClick(int mouseLeft, int mouseTop)
        {
            MoveCursorAbsolute(mouseLeft, mouseTop);
        }

        private void LayoutScreen()
        {
            var levelBlockHeight = (int) (screen.Height * 0.8);
            var levelBlockWidth = (int) (screen.Width * 0.6);
            var statsBlockHeight = (int) (levelBlockHeight * 0.6);

            levelViewportPanel = new LevelViewportPanel(new ScreenZone(-1, -1, levelBlockWidth, levelBlockHeight), 
                screen, () => level);
            messagesPanel = new MessagesPanel(new ScreenZone(-1, levelBlockHeight - 2, screen.Width + 2, screen.Height - levelBlockHeight + 2), 
                screen);
            statsPanel = new StatsPanel(new ScreenZone(levelBlockWidth - 2, -1, screen.Width - levelBlockWidth + 2, statsBlockHeight), 
                screen, () => level?.Player);
            infoPanel = new InfoPanel(new ScreenZone(levelBlockWidth - 2, statsBlockHeight - 2, screen.Width - levelBlockWidth + 2, levelBlockHeight - statsBlockHeight + 1), 
                screen, () => level, () => levelViewportPanel.TranslateCoords(cursorLeft - levelViewportPanel.ClientZone.Left, cursorTop - levelViewportPanel.ClientZone.Top));
        }
        
        private void DrawInterface()
        {
            levelViewportPanel.RedrawBorder();
            messagesPanel.RedrawBorder();
            statsPanel.RedrawBorder();
            infoPanel.RedrawBorder();
        }

        private void InitCursorPosition()
        {
            levelViewportPanel.GetPlayerScreenCoords(out cursorLeft, out cursorTop);

            Console.CursorLeft = cursorLeft;
            Console.CursorTop = cursorTop;
        }

        private void DisplayHelp()
        {
            var helpPanel = new HelpPanel(screen);

            helpPanel.RedrawContents();
            screen.Render(helpPanel.Zone);

            Console.ReadKey(true);

            screen.Clear();
            DrawInterface();
            RenderLevel(level);
        }

        private void MoveCursor(int leftDelta, int topDelta)
        {
            MoveCursorAbsolute(cursorLeft + leftDelta, cursorTop + topDelta);
        }

        private void MoveCursorAbsolute(int left, int top)
        {
            Console.CursorLeft = cursorLeft = Math.Max(0, Math.Min(screen.Width, left));
            Console.CursorTop = cursorTop = Math.Max(0, Math.Min(screen.Height, top));

            infoPanel.RedrawContents();
            screen.Render(infoPanel.ClientZone);
        }

        private int cursorLeft;
        private int cursorTop;

        private LevelViewportPanel levelViewportPanel;
        private MessagesPanel messagesPanel;
        private StatsPanel statsPanel;
        private InfoPanel infoPanel;
        
        private Level level;

        private readonly ITextScreen screen;
        private readonly ConsoleMouseDriver mouseDriver;
    }
}
