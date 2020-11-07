using System.Threading.Tasks;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.Rma;
using Germadent.WebApi.DataAccess.UserManagement;
using Germadent.WebApi.Entities.Conversion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Germadent.WebApi
{
    public class ChatHub : Hub
    {
        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }

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
            services.AddSignalR();
            services.AddSingleton<IServiceConfiguration, ServiceConfiguration>();
            services.AddSingleton<IRmaDbOperations, RmaDbOperations>();
            services.AddSingleton<IAddWorOrderCommand, AddWorkOrderCommand>();
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
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}