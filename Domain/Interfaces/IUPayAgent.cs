namespace UPay.Domain.Interfaces
{
    public interface IUPayAgent
    {
        void Start();
        void Run(object state);
        void Stop();
    }
}
