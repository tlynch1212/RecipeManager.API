using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeManager.Core;
using RecipeManager.Core.Import;
using RecipeManager.Core.Repositories;

namespace RecipeManager.API
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
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection")));
            services.AddDbContext<RecipeManagerContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("RecipeManagerContext")));
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IImportStatusRepository, ImportStatusRepository>();
            services.AddScoped<IImportJobRepository, ImportJobRepository>();
            services.AddScoped<IImportService, ImportService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
