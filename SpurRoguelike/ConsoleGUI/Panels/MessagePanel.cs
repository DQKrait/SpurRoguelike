namespace SpurRoguelike.ConsoleGUI.Panels
{
    internal class MessagePanel : CenteredPanel
    {
        public MessagePanel(ITextScreen screen, ConsoleMessage message)
            : base(screen, BlankSize, message.Text.Length, 1)
        {
            this.message = message;
        }
        
        public override void RedrawContents()
        {
            Screen.Fill(ClientZone, new ConsoleCharacter(' ', message.BackgroundColor, message.BackgroundColor));
            DrawFullWidthMessage(BlankSize, BlankSize, message);
        }

        private readonly ConsoleMessage message;

        private const int BlankSize = 3;
    }
}