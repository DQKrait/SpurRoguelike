using System.Collections.Generic;

namespace SpurRoguelike.ConsoleGUI.Panels
{
    internal class MessagesPanel : Panel
    {
        public MessagesPanel(ScreenZone zone, ITextScreen screen) 
            : base(zone, screen)
        {
            messages = new List<ConsoleMessage>();
        }

        public void AddMessage(ConsoleMessage message)
        {
            messages.Add(message);
            if (messages.Count > ClientZone.Height)
                messages.RemoveAt(0);
        }

        public override void RedrawContents()
        {
            for (int i = 0; i < messages.Count; i++)
                DrawFullWidthMessage(0, i, messages[i]);
        }

        private readonly List<ConsoleMessage> messages;
    }
}