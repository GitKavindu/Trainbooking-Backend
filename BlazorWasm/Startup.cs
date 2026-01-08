using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Interfaces;
using WasmServices;
using BlazorComponents.Service;

namespace BlazorWasm
{
    public class Startup
    {
        public async Task Configure(WebAssemblyHostBuilder builder)
        {
            //Get the appsettings
            HttpClient http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

            AppSettings appSettings = await http.GetFromJsonAsync<AppSettings>("appsettings.json");

            if(appSettings.ServerRendered==false)
            {
                builder.RootComponents.Add<App>("#app");              // Main app
                builder.RootComponents.Add<HeadOutlet>("head::after"); // Dynamic <head> elements
            }

            appSettings.FrontendeUrl = builder.HostEnvironment.BaseAddress;
            
            HttpClient trainBookingBackend = new HttpClient(){ BaseAddress = new Uri(appSettings.BackendeUrl) };

            //service dependencies
            builder.Services.AddScoped<IUserService>(c => new WasmUserService(trainBookingBackend));
            builder.Services.AddScoped<IStationService>(c => new WasmStationService(trainBookingBackend));

            //Blazor Component dependecies
            builder.Services.AddScoped<Ixu>(c => new Xu(false));
            builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<SharedService>();
            builder.Services.AddScoped<DeviceService>();


            if (builder.HostEnvironment.IsDevelopment())
            {
                Console.WriteLine("Blazor WASM running in Development mode.");
                Console.WriteLine(appSettings.BackendeUrl);
                Console.WriteLine(appSettings.FrontendeUrl);
            }
            else
            {
                Console.WriteLine("Blazor WASM running in Production mode.");
            
            }
        }
    }

}

