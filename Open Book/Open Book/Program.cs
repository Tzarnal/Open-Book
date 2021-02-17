using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorFluentUI;
using Open_Book.Services;
using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace Open_Book
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            builder.Services.AddScoped(_ => httpClient);
            builder.Services.AddBlazorFluentUI();

            //Library Services
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredSessionStorage();

            //Open Book services
            var blogService = new BlogService();
            await blogService.Initialize(httpClient);
            builder.Services.AddSingleton<BlogService>(blogService);

            var gameStateService = new GameStateService();
            builder.Services.AddSingleton<GameStateService>(gameStateService);

            var appSettingService = new AppSettingService();
            builder.Services.AddSingleton<AppSettingService>(appSettingService);

            await builder.Build().RunAsync();
        }
    }
}