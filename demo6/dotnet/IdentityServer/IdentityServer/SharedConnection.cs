using System;
using StackExchange.Redis;

namespace IdentityServer
{
    public class SharedConnection
    {
        private static ConnectionMultiplexer _muxer;

        public static ConnectionMultiplexer GetMuxer(string connectionString) {
            //string connectionString = Configuration.GetConnectionString("RedisConnectionString");
            if (String.IsNullOrEmpty(connectionString))
                return null;

            if (_muxer == null) {
                _muxer = ConnectionMultiplexer.Connect(connectionString);
                _muxer.PreserveAsyncOrder = false;
            }

            return _muxer;
        }
    }
}