using System.Net;
using System.Net.Sockets;

using UPay.Data.Interfaces;
using UPay.Operations;

namespace UPay.Data.Layer
{
    public class PointOfSale : IPointOfSale
    {
        public const int RequestSize = 4;

        private readonly ILogger _logger;

        private Socket _posListener;
        private Socket _posConnection;

        public PointOfSale(ILogger logger)
        {
            _logger = logger;
        }

        public void Accept()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);

            _posListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _posListener.Bind(endpoint);
            _posListener.Listen(backlog: 5);
            _logger.Info("Waiting for the POS to connect...");
            _posConnection = _posListener.Accept();
            _logger.Info("POS has connected...");
        }

        public byte[] Receive(int size)
        {
            var bytesToRead = size;
            var data = new byte[bytesToRead];
            var bytesRead = default(int);
            var offset = default(int);

            while (bytesRead < bytesToRead)
            {
                bytesRead = _posConnection.Receive(data, offset, bytesToRead, SocketFlags.None);
                if (bytesRead > 0)
                {
                    bytesToRead -= bytesRead;
                    offset += bytesRead;
                }
                else
                {
                    return null;
                }
            }

            return data;
        }

        public int Send(byte[] data)
        {
            return _posConnection.Send(data);
        }

        public void Shutdown()
        {
            if (_posConnection != null)
            {
                _posConnection.Shutdown(SocketShutdown.Both);
                _posConnection.Close();
            }

            if(_posListener != null)
            {
                _posListener.Close();
            }

            _posConnection = null;
            _posListener = null;
        }

        #region IDisposable support

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Shutdown();
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
