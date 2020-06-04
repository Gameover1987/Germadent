using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities.Conversion;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Germadent.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IServiceConfiguration, ServiceConfiguration>();
            services.AddSingleton<IRmaDbOperations, RmaDbOperations>();
            services.AddSingleton<IUmcDbOperations, UmcDbOperations>();
            services.AddSingleton<IRmaEntityConverter, RmaEntityConverter>();
            services.AddSingleton<IUmcEntityConverter, UmcEntityConverter>();
            services.AddSingleton<IFileManager, FileManager>();
            services.AddSingleton<ILogger, Logger>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}