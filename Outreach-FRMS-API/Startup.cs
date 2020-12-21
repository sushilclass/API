using System;
using System.Collections.Generic;
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
using Outreach_FRMS_BL;
using Outreach_FRMS_DataLayer;
using Microsoft.EntityFrameworkCore;
using Outreach_FRMS_API.Helpers;
using Serilog;
using System.IO;
using Outreach_FRMS_LogManager;
using Outreach_FRMS_Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Outreach_FRMS_API
{
    public class Startup
    {
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               
            .AddJwtBearer(options =>
            {
                options.ClaimsIssuer = "myapi.com";
                options.Audience = "myapi.com";
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //Required else token will fail to be validated and auth will fail
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a random string which should come from appsettings")),
                    ValidateLifetime = true,
                    ValidIssuer = "myapi.com",
                    ValidateIssuer = true,
                };
                options.RequireHttpsMetadata = false;
            });

            // services.AddOptions();
            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // configure the log write setting on and off
            services.Configure<ConfigLog>(Configuration.GetSection("ConfigLog"));
            //Configure the Email Sending setting
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<LogManagers>();
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            var connection = this.Configuration.GetConnectionString("DatabaseConnection");
            services.AddDbContext<OutreachFRMSDBContext>(options => options.UseSqlServer(connection));
            services.AddTransient<OutreachFRMSDBContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            loggerFactory.AddFile(Path.GetDirectoryName(System.Reflection
                            .Assembly.GetExecutingAssembly().Location) + "/Logs/Trace-{Date}.txt", LogLevel.Trace);
            loggerFactory.AddFile(Path.GetDirectoryName(System.Reflection
                      .Assembly.GetExecutingAssembly().Location) + "/Logs/Error-{Date}.txt", LogLevel.Error);

            loggerFactory.AddSerilog();

            // custom jwt auth middleware
           // app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=UserSignUp}/{id?}");
            });
        }
    }
}
