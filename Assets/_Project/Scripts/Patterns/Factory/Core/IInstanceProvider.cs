public interface IInstanceProvider<out T>
{
    T GetInstance();
}
