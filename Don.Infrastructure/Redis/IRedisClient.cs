using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Infrastructure.Redis
{
    public interface IRedisClient
    {
        IDatabase GetDatabase(int db = 0);

        IServer GetServer(int endPointIndex = 0);

        ISubscriber GetSubscriber();
    }
}
