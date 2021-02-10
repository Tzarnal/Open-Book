using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Markdig;
using System.Linq;
using Open_Book.Services;
using Open_Book.Data;
using System;

namespace Open_Book.Pages
{
    public partial class BlogPost
    {
        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        private IBlogService BlogService { get; set; }

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
            var blogPost = BlogService.BlogPosts.Where(p => p.Url == BlogPostID).FirstOrDefault();

            Console.WriteLine(BlogPostID);
            Console.WriteLine(blogPost == null);

            Console.WriteLine(BlogService.BlogPosts[0].Url);

            if (blogPost == null)
            {
                blogPost = new MDPost();
                blogPost.File = "blog/TestPost.md";
            }

            await ReadPostContentAsync(blogPost.File);
            await ReadPostFrontMatterAsync(blogPost.File);
        }

        private async Task ReadPostContentAsync(string filePath)
        {
            var markdown = await Http.GetStringAsync(filePath);

            var markDigPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseYamlFrontMatter()
                .UseEmojiAndSmiley()
                .Build();

            BlogPostContent = Markdown.ToHtml(markdown, markDigPipeline);
        }

        private async Task ReadPostFrontMatterAsync(string filePath)
        {
            BlogPostTitle = "This Blog Post Could Not Be Found";

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