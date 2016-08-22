using SpurRoguelike.Core;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.ConsoleGUI
{
    internal class ConsolePlayerController : IPlayerController
    {
        public ConsolePlayerController(ConsoleGui gui)
        {
            this.gui = gui;
        }

        public Turn MakeTurn(LevelView levelView, IMessageReporter messageReporter)
        {
            var turn = gui.AskPlayerInput();
            if (turn == null)
                gui.Exit();

            return turn;
        }

        private readonly ConsoleGui gui;
    }
}