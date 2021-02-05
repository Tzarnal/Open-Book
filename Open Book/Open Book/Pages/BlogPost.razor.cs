using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Markdig;
using System.Net.Http;
using System.Collections.Generic;

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
        public string BlogPostCategory { get; set; }

        [Parameter]
        public string BlogPostContent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ReadPostContentAsync(@"blog/TestPost.md");
            await ReadPostFrontMatterAsync(@"blog/TestPost.md");
        }

        private async Task ReadPostContentAsync(string filePath)
        {
            var markdown = await Http.GetStringAsync(filePath);

            var markDigPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseYamlFrontMatter()
                .UseEmojiAndSmiley()
                .Build();

            var markDig = Markdown.ToHtml(markdown, markDigPipeline);

            BlogPostContent = markDig;
        }

        private async Task ReadPostFrontMatterAsync(string filePath)
        {
            BlogPostTitle = "Missing Blog Post Title";

            var markdownFile = await Http.GetStringAsync(filePath);
            var markdownFileLines = markdownFile.Split("\r\n");

            if (markdownFileLines[0] != "---")
            {
                return;
            }

            var frontMatter = new Dictionary<string, string>();

            var i = 1;
            var line = markdownFileLines[i];
            while (line != "---")
            {
                var chunks = line.Split(':');

                if (chunks.Length != 2)
                {
                    continue;
                }

                frontMatter.Add(chunks[0].Trim().ToLowerInvariant(),
                                chunks[1].Trim());

                i++;
                line = markdownFileLines[i];
            }

            if (frontMatter.ContainsKey("title"))
            {
                BlogPostTitle = frontMatter["title"];
            }

            if (frontMatter.ContainsKey("category"))
            {
                BlogPostCategory = frontMatter["category"];
            }
        }
    }
}