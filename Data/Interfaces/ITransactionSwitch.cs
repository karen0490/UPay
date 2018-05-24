namespace UPay.Data.Interfaces
{
    public interface ITransactionSwitch
    {
        void Send(byte[] request);
        byte[] Receive();
    }
}
