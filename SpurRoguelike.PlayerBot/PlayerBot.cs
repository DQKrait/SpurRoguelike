using System;
using System.Linq;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.PlayerBot
{
    public class PlayerBot : IPlayerController
    {
        public Turn MakeTurn(LevelView levelView)
        {
            if (levelView.Random.NextDouble() < 0.1)
                return Turn.None;

            var nearbyMonster = levelView.Monsters.FirstOrDefault(m => IsInAttackRange(levelView.Player.Location, m.Location));

            if (nearbyMonster.HasValue)
                return Turn.Attack(AttackToDirection(nearbyMonster.Location - levelView.Player.Location));

            return Turn.Step((StepDirection)levelView.Random.Next(4));
        }

        private static bool IsInAttackRange(Location a, Location b)
        {
            var offset = b - a;
            return Math.Abs(offset.XOffset) <= 1 && Math.Abs(offset.YOffset) <= 1;
        }

        private static AttackDirection AttackToDirection(Offset attack)
        {
            if (attack.XOffset == 0 && attack.YOffset == -1)
                return AttackDirection.North;
            if (attack.XOffset == 1 && attack.YOffset == 0)
                return AttackDirection.East;
            if (attack.XOffset == 0 && attack.YOffset == 1)
                return AttackDirection.South;
            if (attack.XOffset == -1 && attack.YOffset == 0)
                return AttackDirection.West;
            if (attack.XOffset == 1 && attack.YOffset == -1)
                return AttackDirection.NorthEast;
            if (attack.XOffset == 1 && attack.YOffset == 1)
                return AttackDirection.SouthEast;
            if (attack.XOffset == -1 && attack.YOffset == 1)
                return AttackDirection.SouthWest;
            if (attack.XOffset == -1 && attack.YOffset == -1)
                return AttackDirection.NorthWest;
            return 0;
        }
    }
}
