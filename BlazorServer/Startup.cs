using BlazorServer.Components;
using Service;
using Infrastructure;
using Interfaces;
using BlazorComponents.Service;
using WasmServices;

namespace BlazorServer
{
    public class Startup
    {   
        private IConfiguration _configuration { get; }
        private static string _connectionString { get; set; }
        //private static var x;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //  x=System.IO.Directory.GetCurrentDirectory();
             Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            var builder = new ConfigurationBuilder()
                            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorComponents().AddInteractiveServerComponents(options =>
            {
                options.DetailedErrors = true;
            })
            .AddInteractiveWebAssemblyComponents();

            //Infrastucture dependencies
            services.AddScoped<IDbConnectRepo>((c)=> new DbConnectRepo(_connectionString));
            services.AddScoped<IUserDbRepo,UserDbRepo>();
            services.AddScoped<IStationDbRepo,StationDbRepo>();
            services.AddScoped<IAdminDbRepo,AdminDbRepo>();

            //Service dependecies
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IStationService,StationService>();

            //Blazor Component dependecies
            services.AddScoped<Ixu>(c => new Xu(true));
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<SharedService>();
            services.AddScoped<DeviceService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            //app.UseHttpsRedirection();           

            app.UseRouting();

            app.UseAntiforgery();

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapStaticAssets(); 

                endpoints.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode()
                    .AddAdditionalAssemblies(typeof(BlazorWasm.Program).Assembly);                
                
            });
        }
    }
}
