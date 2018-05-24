using System;

namespace UPay.Data.Interfaces
{
    public interface IPointOfSale : IDisposable
    {
        void Accept();
        byte[] Receive(int size);
        int Send(byte[] data);
        void Shutdown();
    }
}
