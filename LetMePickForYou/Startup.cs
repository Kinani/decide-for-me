using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LetMePickForYou
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async context =>
            {
                Random r = new Random();
                context.Response.Headers.Add("content-type", "text/html");

                await context.Response.WriteAsync("<html><body>");
                await context.Response.WriteAsync("<h1>Hi, Let me make your decisions for you.</h1>");
                await context.Response.WriteAsync("<h2>How this works?</h1>");
                await context.Response.WriteAsync(@"<h3>You just add your options to the URL, defined as 'variables' like this:</h3>");
                await context.Response.WriteAsync(@"<h3>http://lkinani.com/decide/?option=movie&option2=documentary&orjust=study</h3>");
                await context.Response.WriteAsync("<br>");
                await context.Response.WriteAsync("<ul>");
                foreach (var v in context.Request.Query)
                {
                    string str = v.Value; 
                    await context.Response.WriteAsync($"<li>{v.Key} - {str}</li>");
                }
                await context.Response.WriteAsync("</ul>");
                if(context.Request.Query.Count > 0)
                {
                    int decision = r.Next(0,context.Request.Query.Count+1);
                    while (decision == 0)
                    {
                        decision = r.Next(0,context.Request.Query.Count+1);
                    }
                    await context.Response.WriteAsync($"<h3>I pick for you {context.Request.Query.ElementAt(decision - 1).Key} : {context.Request.Query.ElementAt(decision - 1).Value.ToString()}</h3>");
                    await context.Response.WriteAsync("<strong>Now go away. bye.</strong>");
                }
                await context.Response.WriteAsync("<br>");
                await context.Response.WriteAsync("<i>FYI: This feature is called query strings in ASP.NET Core.</i>");
                await context.Response.WriteAsync("</body></html>");
            });
        }
    }
}
