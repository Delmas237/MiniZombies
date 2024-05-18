namespace PlayerLib
{
    public interface ICurrencyController
    {
        public int Coins { get; }

        public void Add(int amount);
        public void Spend(int amount);
    }
}
