using System;
using System.Collections.Concurrent;

namespace BookMK.Service
{
    public static class SimpleCache
    {
        private static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();

        public static void AddOrUpdate<T>(string key, T value)
        {
            Cache.AddOrUpdate(key, value, (existingKey, existingValue) => value);
        }

        public static T Get<T>(string key)
        {
            return Cache.TryGetValue(key, out var value) ? (T)value : default;
        }

        public static void Remove(string key)
        {
            Cache.TryRemove(key, out _);
        }
    }
}
