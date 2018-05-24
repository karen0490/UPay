using System;
using System.Text;

namespace UPay.Operations
{
    public class ConsoleLogger : ILogger
    {
        void ILogger.Error(Exception ex, object data)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Error.Write("[E]");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine(ExceptionExtensions.Traverse(ex));
        }

        void ILogger.Error(string message, object data)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Error.Write("[E]");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine(message);
        }

        void ILogger.Info(string message, object data)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write("[I]");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
        }
    }
}
