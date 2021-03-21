using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Application.Common.Models;
using StackExchange.Redis;
using System;

namespace CleanArchitecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var redisTicketConfig = configuration.GetSection("RedisTickets").Get<RedisConfiguration>();
            var redisTicketConfigOptions = new ConfigurationOptions() { AllowAdmin = true };

            foreach (var endpoint in redisTicketConfig.Hosts.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                redisTicketConfigOptions.EndPoints.Add(endpoint);
            }

            services.AddSingleton<IDatabase>(ConnectionMultiplexer.Connect(redisTicketConfigOptions).GetDatabase(Convert.ToInt32(configuration["RedisTickets:Database"])));

            return services;
        }
    }
}
