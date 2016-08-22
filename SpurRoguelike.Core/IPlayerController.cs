using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.Core
{
    public interface IPlayerController
    {
        Turn MakeTurn(LevelView levelView, IMessageReporter messageReporter);
    }
}