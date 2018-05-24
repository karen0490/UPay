using System.ServiceProcess;
using System.Threading;

using UPay.Data.Interfaces;
using UPay.Data.Layer;
using UPay.Domain.Layer;
using UPay.Operations;

namespace UPay
{
    public partial class UPayService : ServiceBase
    {
        private IPointOfSale _pointOfSale;
        private ITransactionSwitch _transactionSwitch;
        private ILogger _logger;
        private UPayAgent _agent;

        public UPayService()
        {
            InitializeComponent();
        }

        public void Start()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            _logger = LoggerFactory.GetLogger();
            _logger.Info("UPayService is starting...");
            _pointOfSale = new PointOfSale(_logger);
            _transactionSwitch = new TransactionSwitch(_logger);
            _agent = new UPayAgent(_pointOfSale, _transactionSwitch, _logger);
            _agent.Start();
        }

        protected override void OnStop()
        {
            _logger.Info("UPayService is stopping...");

            if (_agent != null)
            {
                _agent.Stop();
            }
        }
    }
}
