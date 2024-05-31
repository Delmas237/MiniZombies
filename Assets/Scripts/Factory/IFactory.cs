namespace Factory
{
    public interface IFactory<out T>
    {
        public T GetInstance();
    }
}
