using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using System.IO;
using System.Net.Http;

namespace Open_Book.Pages
{
    public partial class BlogPost
    {
        [Inject]
        protected HttpClient Http { get; set; }

        [Parameter]
        public string BlogPostID { get; set; }

        [Parameter]
        public string BlogPostTitle { get; set; }

        [Parameter]
        public string BlogPostContent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ReadPostAsync(@"blog/TestPost.md");
        }

        private async Task ReadPostAsync(string filePath)
        {
            var markdown = await Http.GetStringAsync(filePath);
            var markDig = Markdown.ToHtml(markdown);

            BlogPostContent = markDig;
        }
    }
}