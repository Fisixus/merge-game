namespace Core.Factories.Pools
{
    public interface IPool<T>
    {
        public T Get();
        public void Return(T obj);
    }
}