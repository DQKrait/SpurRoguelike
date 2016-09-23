using System;

namespace SpurRoguelike.Core.Primitives
{
    public struct Offset : IEquatable<Offset>
    {
        public Offset(int xOffset, int yOffset)
        {
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public readonly int XOffset;

        public readonly int YOffset;

        public Offset Normalize()
        {
            return new Offset(Math.Sign(XOffset), Math.Sign(YOffset));
        }

        public Offset SnapToStep(Random random)
        {
            var attackOffset = Normalize();

            if (XOffset == 0 || YOffset == 0)
                return attackOffset;

            return random.NextDouble() < 0.5 ? new Offset(attackOffset.XOffset, 0) : new Offset(0, attackOffset.YOffset);
        }

        public Offset SnapToStep()
        {
            var offset = Normalize();

            if (XOffset == 0 || YOffset == 0)
                return offset;

            return Math.Abs(XOffset) >= Math.Abs(YOffset) ? new Offset(offset.XOffset, 0) : new Offset(0, offset.YOffset);
        }

        public bool IsStep()
        {
            return Math.Abs(XOffset) == 1 && YOffset == 0 || Math.Abs(YOffset) == 1 && XOffset == 0;
        }

        public Offset Turn(int quarters)
        {
            if (!IsStep())
                return this;

            var clockwise = Math.Sign(quarters);
            quarters = Math.Abs(quarters) % 4;

            var xOffset = XOffset;
            var yOffset = YOffset;

            for (int i = 0; i < quarters; i++)
            {
                if (xOffset == 0)
                {
                    xOffset = -clockwise * yOffset;
                    yOffset = 0;
                }
                else
                {
                    yOffset = clockwise * xOffset;
                    xOffset = 0;
                }
            }

            return new Offset(xOffset, yOffset);
        }

        public int Size()
        {
            return Math.Abs(XOffset) + Math.Abs(YOffset);
        }

        public static Offset FromDirection(StepDirection stepDirection)
        {
            return StepOffsets[(int)stepDirection];
        }

        public static Offset FromDirection(AttackDirection attackDirection)
        {
            return AttackOffsets[(int)attackDirection];
        }

        public static readonly Offset[] StepOffsets = { new Offset(0, -1), new Offset(1, 0), new Offset(0, 1), new Offset(-1, 0) };
        public static readonly Offset[] AttackOffsets = { new Offset(0, -1), new Offset(1, -1), new Offset(1, 0), new Offset(1, 1), new Offset(0, 1), new Offset(-1, 1), new Offset(-1, 0), new Offset(-1, -1) };

        public static Offset operator -(Offset offset)
        {
            return new Offset(-offset.XOffset, -offset.YOffset);
        }

        public bool Equals(Offset other)
        {
            return XOffset == other.XOffset && YOffset == other.YOffset;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
                return false;
            return other is Offset && Equals((Offset)other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (XOffset * 397) ^ YOffset;
            }
        }

        public static bool operator ==(Offset left, Offset right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Offset left, Offset right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"X: {XOffset}, Y: {YOffset}";
        }
    }
}