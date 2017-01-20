namespace RequestsForRights.CachePool
{
    public interface ICachePool
    {
        T GetValue<T>(string key);
        void SetValue<T>(string key, T value);
        bool HasValue<T>(string key);
    }
}
