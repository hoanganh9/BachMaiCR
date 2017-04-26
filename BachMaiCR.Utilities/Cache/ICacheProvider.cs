namespace BachMaiCR.Utilities.Cache
{
    public interface ICacheProvider
    {
        void Set(string key, object data, int cacheTime = 60);

        T Get<T>(string key);

        bool IsStored(string key);

        void Remove(string key);

        void Clear();

        void RemoveByTerm(string term);
    }
}