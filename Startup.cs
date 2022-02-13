using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace TanksIO
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3002").AllowCredentials().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(env.ContentRootPath, "WebPage")),
                RequestPath = ""
            });

            app.UseCors();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<Sockets.GameHub>("/API");

                endpoints.MapFallback(async context =>
                {
                    await context.Response.WriteAsync(System.IO.File.ReadAllText("./WebPage/index.html"));
                });
            });
        }
    }
}
