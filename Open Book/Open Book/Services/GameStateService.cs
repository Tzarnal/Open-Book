using System.Collections.Generic;

namespace Open_Book.Services
{
    public class GameStateService
    {
        public List<string> BlogPosts { get; }

        public GameStateService()
        {
            BlogPosts = new();
        }
    }
}