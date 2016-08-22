using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core
{
    public interface IEventReporter
    {
        void ReportMessage(Entity instigator, string message);
        void ReportLevelEnd();
        void ReportGameEnd();
        void ReportNewEntity(Location location, Entity entity);
        void ReportAttack(Pawn attacker, Pawn victim);
        void ReportDamage(Pawn pawn, int damage, Entity instigator);
        void ReportTrap(Pawn pawn);
        void ReportPickup(Pawn pawn, Pickup item);
        void ReportDestroyed(Pawn pawn);
        void ReportUpgrade(Pawn pawn, int attackBonus, int defenceBonus);
    }
}