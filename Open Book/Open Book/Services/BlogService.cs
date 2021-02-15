using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Open_Book.Data;

namespace Open_Book.Services
{
    public class BlogService
    {
        public List<MDPost> BlogPosts { get; set; }

        public List<string> BlogCategories { get => BlogPosts.Select(p => p.Category).Distinct().ToList(); }

        private string _blogIndexFile = @"blog/MdPosts.json";

        public BlogService()
        {
            BlogPosts = new();
        }

        public IEnumerable<MDPost> GetBlogPosts()
        {
            return BlogPosts;
        }

        public IEnumerable<MDPost> GetBlogPosts(List<string> availablePostIds)
        {
            var posts = BlogPosts.Where(p => availablePostIds.Contains(p.Id));
            foreach (var p in posts)
            {
                yield return p;
            }
        }

        public async Task Initialize(HttpClient Http)
        {
            await ReadIndexFile(_blogIndexFile, Http);
        }

        private async Task ReadIndexFile(string fileUrl, HttpClient Http)
        {
            var jsonData = await Http.GetStringAsync(fileUrl);

            BlogPosts = JsonSerializer.Deserialize<List<MDPost>>(jsonData); ;
        }
    }
}