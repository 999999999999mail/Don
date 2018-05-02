using Microsoft.Extensions.DependencyInjection;
using System;

namespace Don.Infrastructure.Redis
{
    public static class RedisServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, Action<RedisOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddSingleton<IRedisClient, RedisClient>();
            services.Configure(setupAction);
            return services;
        }
    }
}
