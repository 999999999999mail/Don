﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Infrastructure.Redis
{
    public class RedisClientOptions
    {
        public string ConnectionString { get; set; }

        public string InstanceName { get; set; }
    }
}
