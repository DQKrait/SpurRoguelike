using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Entities
{
    public abstract class Entity
    {
        protected Entity(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Location Location { get; protected set; }

        public Level Level { get; protected set; }

        public bool IsDestroyed { get; private set; } = false;

        public virtual void Tick()
        {
        }

        public bool Move(Location newLocation, Level newLevel)
        {
            var oldLocation = Location;

            if (!newLevel.Field.Contains(newLocation) || !ProcessMove(newLocation, newLevel))
                return false;

            Location = newLocation;

            Level?.ProcessMove(this, oldLocation, Location);

            if (Level != newLevel)
            {
                Level?.Destroy(this);
                Level = newLevel;
                newLevel.Spawn(newLocation, this);
            }

            return true;
        }

        public bool IsInRange(Entity other, int range)
        {
            return Location.IsInRange(other.Location, range) && !other.IsDestroyed && Level == other.Level;
        }

        protected virtual bool ProcessMove(Location newLocation, Level newLevel)
        {
            return true;
        }

        public virtual void Destroy()
        {
            IsDestroyed = true;
            Level.Destroy(this);
        }
    }
}