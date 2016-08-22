using System;

namespace SpurRoguelike.ConsoleGUI.Panels
{
    internal abstract class CenteredPanel : Panel
    {
        protected CenteredPanel(ITextScreen screen, int blankSize, int contentWidth, int contentHeight)
            : base(GetDisposition(screen.Width, screen.Height, blankSize, contentWidth, contentHeight), screen)
        {
        }

        public override void RedrawBorder()
        {
        }

        private static ScreenZone GetDisposition(int screenWidth, int screenHeight, int blankSize, int contentWidth, int contentHeight)
        {
            var width = 2 + blankSize * 2 + contentWidth;
            var height = 2 + blankSize * 2 + contentHeight;

            return new ScreenZone(Math.Max(0, screenWidth / 2 - width / 2), Math.Max(0, screenHeight / 2 - height / 2), width, height);
        }
    }
}