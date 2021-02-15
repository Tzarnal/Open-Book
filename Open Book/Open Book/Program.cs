using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorFluentUI;
using Open_Book.Services;

namespace Open_Book
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            builder.Services.AddScoped(sp => httpClient);
            builder.Services.AddBlazorFluentUI();

            var blogService = new BlogService();
            await blogService.Initialize(httpClient);
            builder.Services.AddSingleton<BlogService>(blogService);

            await builder.Build().RunAsync();
        }
    }
}