using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NakedBank.Front.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NakedBank.Front
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<INakedService, NakedService>()
                .AddScoped<IHttpService, HttpService>()
                .AddScoped<ILocalStorageService, LocalStorageService>();

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["apiUrl"]) });

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
