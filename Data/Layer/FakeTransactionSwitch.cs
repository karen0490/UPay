using System;
using System.Text;

using UPay.Data.Interfaces;
using UPay.Operations;

namespace UPay.Data.Layer
{
    public class FakeTransactionSwitch : ITransactionSwitch
    {
        private readonly ILogger _logger;

        public FakeTransactionSwitch(ILogger logger)
        {
            _logger = logger;
        }

        public byte[] Receive()
        {
            _logger.Info("Inside TransactionSwitch.Receive");

            var artificialResponse = string.Format("Hello, world! {0}", DateTime.Now);

            return Encoding.ASCII.GetBytes(artificialResponse);
        }

        public void Send(byte[] data)
        {
            var message = string.Format("Inside TransactionSwitch.Send: {0}", Encoding.ASCII.GetString(data));

            _logger.Info(message);
        }
    }
}
