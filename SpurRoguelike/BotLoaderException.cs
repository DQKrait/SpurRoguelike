using System;

namespace SpurRoguelike
{
    internal class BotLoaderException : Exception
    {
        public BotLoaderException(string message) 
            : base(message)
        {
        }

        public BotLoaderException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}