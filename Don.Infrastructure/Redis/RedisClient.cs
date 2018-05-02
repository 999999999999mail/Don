using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;

namespace Don.Infrastructure.Redis
{
    public class RedisClient : IRedisClient, IDisposable
    {
        private readonly RedisOptions _options;
        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;

        public RedisClient(IOptions<RedisOptions> options)
        {
            _options = options.Value;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        private ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_options.InstanceName, p => ConnectionMultiplexer.Connect(_options.ConnectionString));
        }

        public IDatabase GetDatabase(int db = 0)
        {
            return GetConnect().GetDatabase(db);
        }

        public IServer GetServer(int endPointIndex = 0)
        { 
            var options = ConfigurationOptions.Parse(_options.ConnectionString);
            return GetConnect().GetServer(options.EndPoints[endPointIndex]);
        }

        public ISubscriber GetSubscriber()
        {
            return GetConnect().GetSubscriber();
        }

        public async void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    await item.CloseAsync();
                }
            }
        }
    }
}
