public interface IInstanceProvider<out T>
{
    public T GetInstance();
}
