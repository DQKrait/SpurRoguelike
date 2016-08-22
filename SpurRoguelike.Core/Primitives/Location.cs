using System;

namespace SpurRoguelike.Core.Primitives
{
    public struct Location : IEquatable<Location>
    {
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly int X;

        public readonly int Y;

        public bool IsInRange(Location other, int range)
        {
            var offset = this - other;
            return Math.Abs(offset.XOffset) <= range && Math.Abs(offset.YOffset) <= range;
        }

        public static Location operator +(Location location, Offset offset)
        {
            return new Location(location.X + offset.XOffset, location.Y + offset.YOffset);
        }

        public static Location operator -(Location location, Offset offset)
        {
            return new Location(location.X - offset.XOffset, location.Y - offset.YOffset);
        }

        public static Offset operator -(Location location1, Location location2)
        {
            return new Offset(location1.X - location2.X, location1.Y - location2.Y);
        }

        public bool Equals(Location other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
                return false;
            return other is Location && Equals((Location)other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Location left, Location right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Location left, Location right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}