using System;

namespace UPay.Operations
{
    public interface ILogger
    {
        void Error(Exception ex, object data = null);
        void Error(string message, object data = null);
        void Info(string message, object data = null);
    }
}
