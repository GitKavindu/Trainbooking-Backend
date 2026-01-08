using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorWasm;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var startup = new Startup();
            await startup.Configure(builder);

            await builder.Build().RunAsync();
        }
    }
}

