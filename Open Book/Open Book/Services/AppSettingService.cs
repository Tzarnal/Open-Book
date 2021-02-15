using System.Collections.Generic;

namespace Open_Book.Services
{
    public class AppSettingService
    {
        public Dictionary<string, string> Settings;

        public AppSettingService()
        {
            Settings = new();
        }
    }
}