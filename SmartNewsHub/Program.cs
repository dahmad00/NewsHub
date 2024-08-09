using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tech360.Data;
using Tech360.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Tech360.Jobs;  // Ensure your jobs namespace is correct

namespace Tech
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure DbContext with SQL Server
            builder.Services.AddDbContext<Tech360Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<Tech360Context>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            });

            // Retrieve the API key from the configuration
            var apiKey = builder.Configuration["NewsApi:ApiKey"];

            // Register the HistoricalNewsFetcher service
            builder.Services.AddTransient<HistoricalNewsFetcher>(provider =>
                new HistoricalNewsFetcher(provider.GetRequiredService<Tech360Context>(), apiKey));

            // Comment out Quartz services related to FetchNewsJob
            /*
            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.ScheduleJob<FetchNewsJob>(trigger => trigger
                    .WithIdentity("FetchNewsTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(3) // Runs every 3 minutes, adjust as needed
                        .RepeatForever())
                    .WithDescription("Fetches news every 3 minutes."));
            });

            // Add Quartz hosted service
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            */

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
