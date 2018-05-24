using System;

namespace UPay.Operations
{
    public class LoggerFactory
    {
        public static ILogger GetLogger()
        {
            if (Environment.UserInteractive)
            {
                return new ConsoleLogger();
            }
            else
            {
                return new WindowsEventLogger();
            }
        }
    }
}
