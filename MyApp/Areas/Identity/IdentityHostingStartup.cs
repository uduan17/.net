using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Data;

[assembly: HostingStartup(typeof(MyApp.Areas.Identity.IdentityHostingStartup))]
namespace MyApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            // builder.ConfigureServices((context, services) => {
            // services.AddDbContext<MyAppContext>(options =>
            //  options.UseSqlServer(
            //context.Configuration.GetConnectionString("MyAppContextConnection")));

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //  .AddEntityFrameworkStores<MyAppContext>();
            // });
        }
    }
}