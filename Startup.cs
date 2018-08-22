using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginReg.Models;  // this namespace's models
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;  //added this
using Microsoft.Extensions.DependencyInjection;
// below 2 namespaces allow us to use the separated connection string in our appsettings.json file that holds 
// sensitive info
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LoginReg
{
    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            //creating an instance of the ConfigurationBuilder object in order to start using the configuration API
            var builder = new ConfigurationBuilder() //now we can tell this instance where app's configuration settings will be stored
                .SetBasePath(env.ContentRootPath)   //importing environment path
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) 
                .AddEnvironmentVariables();   
                Configuration = builder.Build();  //call to build method, tells ConfigurationBuilder to evaluate all config. sources & build up config. obj. to gain access to those config. sources
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Dependency Injection Container - add framework services here     
            services.AddSession();     
            // Adding the Dbcontext type to container using the AddDbContext<TContext> method. This method makes 
            // DbContext type (TContext) & corresponding DbContextOptions<TContext> available for injection 
            // from service container
            services.AddDbContext<MyDbContext>(options => options.UseMySql(Configuration["DBInfo:ConnectionString"])); // add this with EF Core
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
