using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.Core
{
    public class Level
    {
        public Level(Field field, int seed)
        {
            Field = field;
            Entities = new List<Entity>();
            Random = new Random(seed);

            entitiesIndex = new EntitiesIndex(field.Width, field.Height);
        }

        public LevelView CreateView()
        {
            return new LevelView(this);
        }

        public void Destroy(Entity entity)
        {
            Entities.Remove(entity);

            if (entitiesIndex.Contains(entity.Location))
                entitiesIndex[entity.Location] = null;
        }

        public void Spawn(Location location, Entity entity)
        {
            if (Entities.Contains(entity))
                return;

            if (!entitiesIndex.Contains(location))
                return;

            Entities.Add(entity);
            entity.Move(location, this);
            entitiesIndex[location] = entity;

            if (entity is Player)
                Player = (Player) entity;

            Player?.EventReporter?.ReportNewEntity(location, entity);
        }
        
        public TEntity GetEntity<TEntity>(Location location) where TEntity : Entity
        {
            if (!entitiesIndex.Contains(location))
                return null;
            
            return entitiesIndex[location] as TEntity;
        }

        public void SetNextLevel(Level level)
        {
            nextLevel = level;
        }

        public void Complete()
        {
            IsCompleted = true;

            Player.EventReporter?.ReportLevelEnd();

            if (nextLevel == null)
            {
                Player.EventReporter?.ReportGameEnd();

                Player.Destroy();
                return;
            }

            Player.Move(nextLevel.Field.PlayerStart, nextLevel);
        }

        public void Tick()
        {
            Player.Tick();

            foreach (var entity in Entities.ToList())
            {
                if (entity == Player)
                    continue;

                entity.Tick();
            }
        }

        public void ProcessMove(Location from, Location to)
        {
            entitiesIndex.Move(from, to);
        }

        public Field Field { get; }

        public Player Player { get; private set; }

        public List<Entity> Entities { get; }

        public Random Random { get; }

        public IEnumerable<Monster> Monsters => Entities.OfType<Monster>();

        public IEnumerable<Pawn> Pawns => Entities.OfType<Pawn>();

        public IEnumerable<Item> Items => Entities.OfType<Item>();

        public IEnumerable<HealthPack> HealthPacks => Entities.OfType<HealthPack>();

        public bool IsCompleted { get; private set; }

        private readonly EntitiesIndex entitiesIndex;

        private Level nextLevel;
    }
}