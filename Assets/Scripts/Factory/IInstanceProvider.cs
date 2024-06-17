namespace Factory
{
    public interface IInstanceProvider<out T>
    {
        public T GetInstance();
    }
}
