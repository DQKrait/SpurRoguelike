namespace SpurRoguelike.ConsoleGUI
{
    internal interface ITextScreen
    {
        void Clear();

        void Put(int left, int top, ConsoleCharacter character);

        void Fill(ScreenZone zone, ConsoleCharacter character);

        void Write(int left, int top, ConsoleMessage message);

        void Write(int left, int top, ConsoleMessage message, int redrawLength);

        void Render();

        void Render(ScreenZone zone);

        int Width { get; }

        int Height { get; }
    }
}