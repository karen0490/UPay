using System;
using System.Text;
using System.Threading;

using UPay.Data.Interfaces;
using UPay.Data.Layer;
using UPay.Domain.Interfaces;
using UPay.Operations;

namespace UPay.Domain.Layer
{
    public class UPayAgent : IUPayAgent, IDisposable
    {
        private readonly IPointOfSale _pointOfSale;
        private readonly ITransactionSwitch _transactionSwitch;
        private readonly ILogger _logger;

        private int _posRequestSize;
        private byte[] _posRequestData;
        private byte[] _transactionSwitchResponseData;

        public UPayAgentState AgentState { get; private set; }

        public UPayAgent(IPointOfSale pointOfSale, ITransactionSwitch transactionSwitch, ILogger logger)
        {
            _pointOfSale = pointOfSale;
            _transactionSwitch = transactionSwitch;
            _logger = logger;

            AgentState = UPayAgentState.AcceptPointOfSaleConnection;
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Run), null);
        }

        public void Run(object state)
        {
            while (true)
            {
                switch (AgentState)
                {
                    case UPayAgentState.AcceptPointOfSaleConnection:
                        AcceptPointOfSaleConnection();
                        break;

                    case UPayAgentState.ReceivePointOfSaleRequestSize:
                        ReceivePointOfSaleRequestSize();
                        break;

                    case UPayAgentState.ReceivePointOfSaleRequestData:
                        ReceivePointOfSaleRequestData();
                        break;

                    case UPayAgentState.SendPointOfSaleRequestToTransactionSwitch:
                        SendPointOfSaleRequestDataToTransactionSwitch();
                        break;

                    case UPayAgentState.ReceiveTransactionSwitchResponse:
                        ReceiveTransactionSwitchResponse();
                        break;

                    case UPayAgentState.SendTransactionSwitchResponseToPointOfSale:
                        SendTransactionSwitchResponseToPointOfSale();
                        break;

                    case UPayAgentState.ShutdownPointOfSaleConnection:
                        ShutdownPointOfSaleConnection();
                        break;
                }
            }
        }

        public void Stop()
        {
            Dispose();
        }

        private void AcceptPointOfSaleConnection()
        {
            try
            {
                _pointOfSale.Accept();
                AgentState = UPayAgentState.ReceivePointOfSaleRequestSize;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
            }
        }

        private void ReceivePointOfSaleRequestSize()
        {
            try
            {
                var data = default(byte[]);
                var message = default(string);

                _logger.Info("Receiving POS request size...");
                data = _pointOfSale.Receive(PointOfSale.RequestSize);
                if (data != null)
                {
                    _posRequestSize = Convert.ToInt32(Encoding.ASCII.GetString(data));
                    message = string.Format("POS request size received: {0}", _posRequestSize);
                    _logger.Info(message);
                    AgentState = UPayAgentState.ReceivePointOfSaleRequestData;
                }
                else
                {
                    _logger.Error("Protocol Violation: POS request content length");
                    AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
            }
        }

        private void ReceivePointOfSaleRequestData()
        {
            try
            {
                _logger.Info("Receiving POS request data...");
                _posRequestData = _pointOfSale.Receive(_posRequestSize - PointOfSale.RequestSize);
                if (_posRequestData != null)
                {
                    _logger.Info("POS request data received...");
                    AgentState = UPayAgentState.SendPointOfSaleRequestToTransactionSwitch;
                }
                else
                {
                    _logger.Error("Protocol Violation: POS request content length");
                    AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
            }
        }

        private void SendPointOfSaleRequestDataToTransactionSwitch()
        {
            try
            {
                _logger.Info("Sending to the transaction switch...");
                _transactionSwitch.Send(_posRequestData);
                AgentState = UPayAgentState.ReceiveTransactionSwitchResponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
            }
        }

        private void ReceiveTransactionSwitchResponse()
        {
            try
            {
                _logger.Info("Receiving from the transaction switch...");
                _transactionSwitchResponseData = _transactionSwitch.Receive();
                AgentState = UPayAgentState.SendTransactionSwitchResponseToPointOfSale;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
            }
        }

        private void SendTransactionSwitchResponseToPointOfSale()
        {
            try
            {
                _logger.Info("Sending response to the point of sale...");
                _pointOfSale.Send(_transactionSwitchResponseData);
                AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                AgentState = UPayAgentState.ShutdownPointOfSaleConnection;
            }
        }

        private void ShutdownPointOfSaleConnection()
        {
            _pointOfSale.Shutdown();
            AgentState = UPayAgentState.AcceptPointOfSaleConnection;
        }

        #region IDisposable support

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    ShutdownPointOfSaleConnection();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
