using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Interfaces;
using Service;
using Infrastructure;
//using Internal;

namespace TrainBookingSystem.TrainApi
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
                            .AddJsonFile("AppSettings.json");

            _configuration = builder.Build();
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            
            //Add Cors
            services.AddCors(c=>{
                c.AddPolicy("AllowOrigin",options =>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            //Add Serializer
            services.AddControllersWithViews().AddNewtonsoftJson(options=>
                options.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options=>options.SerializerSettings.ContractResolver=new DefaultContractResolver());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TrainBookingSystem", Version = "v1" });
            });

            //Infrastucture dependencies
            services.AddScoped<IDbConnectRepo>((c)=> new DbConnectRepo(_connectionString));
            services.AddScoped<IUserDbRepo,UserDbRepo>(); 
            services.AddScoped<IAdminDbRepo,AdminDbRepo>();
            services.AddScoped<IStationDbRepo,StationDbRepo>();
            services.AddScoped<ITrainDbRepo,TrainDbRepo>();
            services.AddScoped<IApartmentDbRepo,ApartmentDbRepo>();
            services.AddScoped<IJourneyDbRepo,JourneyDbRepo>();
            //services.AddScoped<IBookingDbRepo,BookingDbRepo>();

            //Service dependecies
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IAdminService,AdminService>();
            services.AddScoped<IStationService,StationService>();
            services.AddScoped<ITrainService,TrainService>();
            services.AddScoped<IApartmentService,ApartmentService>();
            services.AddScoped<IJourneyService,JourneyService>();
            //services.AddScoped<IBookingService,BookingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrainBookingSystem v1"));
            }

            //app.UseHttpsRedirection();
            
            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
