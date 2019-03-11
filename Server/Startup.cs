using Common.Interfaces;
using Common.Logging;
using Library1C;
using LibraryAmoCRM;
using LibraryAmoCRM.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using WebApi.Common.Models;
using WebApi.Infrastructure.Filters;
using WebApi.Infrastructure.SerilogEnrichers;
using WebApiLogic;

namespace WebApiFPA
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMemoryCache();

            services.AddScoped<RequestScope>();

            //services.AddScoped<ILoggerService, LoggerService>();

            services.AddSingleton<Connection>(con =>
           {
               return new Connection(
                   Configuration.GetSection("providers:0:AmoCRM:connection:account:name").Value,
                   Configuration.GetSection("providers:0:AmoCRM:connection:account:email").Value,
                   Configuration.GetSection("providers:0:AmoCRM:connection:account:hash").Value
               );
           });

            services.AddScoped( mapper => { return new TypeAdapterConfig(); } );

            services.AddScoped<IDataManager, CrmManager>();

            services.AddScoped( service1C =>
            {
                return new UnitOfWork(
                    Configuration.GetSection( "providers:1:1C:connection:account:user" ).Value,
                    Configuration.GetSection( "providers:1:1C:connection:account:pass" ).Value
                );
            } );

            services.AddSingleton( neo =>
            {
                return new Lazy<ServiceLibraryNeoClient.Implements.DataManager>(
                    () => new ServiceLibraryNeoClient.Implements.DataManager(
                    new System.Uri(Configuration.GetSection("providers:2:Neo:connection:account:uri").Value),
                        Configuration.GetSection("providers:2:Neo:connection:account:user").Value,
                        Configuration.GetSection("providers:2:Neo:connection:account:pass").Value
                    )
                    , true
                );
            } );

            services.AddScoped<BusinessLogic>();

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Connection connection, ILoggerFactory loggerFactory)
        {
            //connection.Auth(null);


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //.Enrich.FromLogContext()
                //.Enrich.With(new ThreadIdEnricher())
                //.Enrich.With(new AssemblyNameEnricher())
                //.Enrich.With(new RequestScopeEnricher(requestScope))
                //.Enrich.WithProperty("~Application", configuration["applicationName"])
                //.Enrich.WithProperty("~Enviroment", configuration["ASPNETCORE_ENVIRONMENT"])
                .WriteTo.Async(a => a.Seq("http://logs.fitness-pro.ru:5341"))
            .CreateLogger();


            loggerFactory
                .AddSerilog()
                .AddConsole();
                

            app.UseCors( builder => builder.AllowAnyOrigin()
                                         .AllowAnyHeader()
                                         .AllowAnyMethod() );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc( routes =>
             {
                 routes.MapRoute( "default", "{controller=Home}/{action=Index}/{id?}" );
             } );
        }
    }
}
