using System;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.ConsoleGUI
{
    internal class ConsoleEventReporter : IEventReporter
    {
        public ConsoleEventReporter(ConsoleGui gui)
        {
            this.gui = gui;
        }

        public void ReportMessage(string message)
        {
            ReportMessage(null, message);
        }

        public void ReportMessage(Entity instigator, string message)
        {
            if (gui.IsOnScreen(instigator))
                gui.DisplayMessage(new ConsoleMessage(message, ConsoleColor.Gray));
        }

        public void ReportLevelEnd()
        {
            gui.DisplayMessage(new ConsoleMessage("You have completed the level!", ConsoleColor.Green));
        }

        public void ReportGameEnd()
        {
            gui.DisplayMessage(new ConsoleMessage("The game is over.", ConsoleColor.White));
        }

        public void ReportNewEntity(Location location, Entity entity)
        {
            gui.DisplayMessage(new ConsoleMessage($"A new {entity.Name} appears!", ConsoleColor.White));
        }

        public void ReportAttack(Pawn attacker, Pawn victim)
        {
            if (gui.IsOnScreen(attacker) || gui.IsOnScreen(victim))
                gui.DisplayMessage(new ConsoleMessage($"{attacker.Name} attacks {victim.Name}!", ConsoleColor.Gray));
        }

        public void ReportDamage(Pawn pawn, int damage, Entity instigator)
        {
            if (gui.IsOnScreen(pawn))
                gui.DisplayMessage(new ConsoleMessage(instigator == null ?
                    $"{pawn.Name} takes {damage} damage." :
                    $"{pawn.Name} takes {damage} damage from {instigator.Name}.", ConsoleColor.Gray));
        }

        public void ReportTrap(Pawn pawn)
        {
            if (gui.IsOnScreen(pawn))
                gui.DisplayMessage(new ConsoleMessage($"{pawn.Name} joins the Mickey Mouse Club!", ConsoleColor.Gray));
        }

        public void ReportPickup(Pawn pawn, Pickup item)
        {
            if (gui.IsOnScreen(pawn))
                gui.DisplayMessage(new ConsoleMessage($"{pawn.Name} picks up {item.Name}.", ConsoleColor.Gray));
        }

        public void ReportDestroyed(Pawn pawn)
        {
            if (gui.IsOnScreen(pawn))
                gui.DisplayMessage(new ConsoleMessage($"{pawn.Name} ceases to exist.", ConsoleColor.DarkRed));
        }

        public void ReportUpgrade(Pawn pawn, int attackBonus, int defenceBonus)
        {
            gui.DisplayMessage(new ConsoleMessage($"{pawn.Name}'s skills grow: attack +{attackBonus}, defence +{defenceBonus}.", ConsoleColor.White));
        }

        private readonly ConsoleGui gui;
    }
}