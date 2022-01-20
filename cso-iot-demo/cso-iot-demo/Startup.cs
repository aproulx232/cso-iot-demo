using System;
using cso_iot_demo;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace cso_iot_demo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = InitializeConfiguration(builder.Services.BuildServiceProvider());

            builder.Services.AddDbContext<AuctionDbContext>(
                options => options.UseSqlServer(config.GetValue<string>("ConnectionStrings:AuctionDb")));
            builder.Services.AddScoped<IRepairItemRepository, RepairItemRepository>();
        }

        protected virtual IConfiguration InitializeConfiguration(IServiceProvider provider)
        {
            var config = provider.GetService<IConfiguration>();
            config = new ConfigurationBuilder()
                .AddConfiguration(config)
                .Build();

            return config;
        }
    }
}