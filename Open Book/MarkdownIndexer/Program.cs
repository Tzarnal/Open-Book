using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Open_Book.Data;

namespace MarkdownIndexer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please supply a directory path to index.");
                return;
            }

            var filePath = args[0];
            var files = GetMDFiles(args[0]);

            List<MDPost> mdPosts = new();

            foreach (var file in files)
            {
                mdPosts.Add(ReadMDFile(file));
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(mdPosts, options);

            File.WriteAllText($"{filePath}/MdPosts.json", jsonString);
        }

        private static IEnumerable<string> GetMDFiles(string searchDirectory)
        {
            if (!Directory.Exists(searchDirectory))
            {
                Console.WriteLine($"Could not index {searchDirectory}. It does not exist.");
                yield break;
            }

            var searchDirectories = Directory.GetDirectories(searchDirectory, "*.*", SearchOption.AllDirectories).ToList();
            searchDirectories.Add(searchDirectory);

            foreach (var dir in searchDirectories)
            {
                var files = Directory.GetFiles(dir, "*.md");

                foreach (var file in files)
                {
                    yield return file;
                }
            }
        }

        private static MDPost ReadMDFile(string filePath)
        {
            var mdPost = new MDPost();

            var markdownFileLines = File.ReadAllLines(filePath);

            if (markdownFileLines[0] != "---")
            {
                return null;
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

            mdPost.File = FileName(filePath);

            if (frontMatter.ContainsKey("title"))
            {
                mdPost.Title = frontMatter["title"];
            }
            else
            {
                mdPost.Title = "Missing Title";
            }

            if (frontMatter.ContainsKey("category"))
            {
                mdPost.Category = frontMatter["category"];
            }
            else
            {
                mdPost.Title = "Default Category";
            }

            if (frontMatter.ContainsKey("id"))
            {
                mdPost.Id = frontMatter["id"];
            }
            else
            {
                var md5 = MD5.Create();
                mdPost.Id = ToHexString(
                    md5.ComputeHash(
                        Encoding.Default.GetBytes(mdPost.File)));
            }

            if (frontMatter.ContainsKey("url"))
            {
                mdPost.Url = frontMatter["url"];
            }
            else
            {
                var url = mdPost.Title;
                url = Regex.Replace(url, @"\W", "_");
                mdPost.Url = url;
            }

            return mdPost;
        }

        private static string FileName(string filePath)
        {
            Console.WriteLine(filePath);

            var fileNameMatch = Regex.Match(filePath, @"(blog\\.+)");

            return fileNameMatch.Value.Replace('\\', '/');
        }

        private static string ToHexString(byte[] bytes, bool upperCase = true)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}