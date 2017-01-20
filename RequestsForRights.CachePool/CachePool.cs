using System;
using System.Collections.Generic;

namespace RequestsForRights.CachePool
{
    public class CachePool: ICachePool
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public T GetValue<T>(string key)
        {
            return _cache.ContainsKey(key) ? (T) _cache[key] : default(T);
        }

        public void SetValue<T>(string key, T value)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
            }
            else
            {
                _cache.Add(key, value);
            }
        }

        public bool HasValue<T>(string key)
        {
            try
            {
                if (!_cache.ContainsKey(key)) return false;
                // ReSharper disable once UnusedVariable
                var val = (T) _cache[key];
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}
