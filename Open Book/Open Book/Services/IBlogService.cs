using System.Collections.Generic;
using Open_Book.Data;

namespace Open_Book.Services
{
    public interface IBlogService
    {
        public List<MDPost> BlogPosts { get; set; }
        public List<string> BlogCategories { get; }
    }
}