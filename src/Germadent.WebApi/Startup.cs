using System.Text;
using System.Threading.Tasks;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.WebApi.Auth;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.DataAccess;
using Germadent.WebApi.DataAccess.Rma;
using Germadent.WebApi.DataAccess.UserManagement;
using Germadent.WebApi.Entities.Conversion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Germadent.WebApi
{
    public class NotificationHub : Hub
    {
        
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
            var key = Encoding.ASCII.GetBytes("SECRET");

            //services.AddAuthentication(x =>
            //    {
            //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(x =>
            //    {
            //        x.RequireHttpsMetadata = true;
            //        x.SaveToken = true;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateIssuer = false,
            //            ValidateAudience = false
            //        };
            //    });
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    };
                });
            services.AddAuthorization();
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

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/NotificationHub");
            });
        }
    }
}