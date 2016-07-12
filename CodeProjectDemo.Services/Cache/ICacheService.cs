namespace CodeProjectDemo.Services.Cache
{
    public interface ICacheService
    {
        T Get<T>(string key);

        void Set<T>(string key, T objectToCache, int minutes = 15);

        T Remove<T>(string key);
    }
}