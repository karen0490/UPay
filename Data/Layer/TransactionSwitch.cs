using System;
using System.Collections.Generic;
using System.Text;
using UPay.Data.Interfaces;
using UPay.Operations;

namespace UPay.Data.Layer
{
    public class TransactionSwitch : ITransactionSwitch
    {
        private readonly ILogger _logger;

        trnRequestValues requestValues;
        localFunctions myFunctions;
        string result;

        public TransactionSwitch(ILogger logger)
        {
            _logger = logger;
            requestValues = new trnRequestValues(_logger);
            myFunctions = new localFunctions(_logger);
        }

        public void Send(byte[] request)
        {
            var data = Encoding.ASCII.GetString(request, 0, request.Length);
            myFunctions.procesaDatos(requestValues, data);
            result = myFunctions.ejecutaLlamada(requestValues);            
        }

        public byte[] Receive()
        {
            byte[] msg = Encoding.ASCII.GetBytes(result);
            return msg;
        }
    }
}
