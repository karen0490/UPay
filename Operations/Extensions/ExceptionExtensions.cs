using System;
using System.Text;

namespace UPay.Operations
{
	public static class ExceptionExtensions
	{
		public static string Traverse(Exception target)
		{
            var message = new StringBuilder(target.StackTrace);

            while (target.InnerException != null)
            {
                target = target.InnerException;
                message.Append(Environment.NewLine + target.StackTrace);
            }

            return message.ToString();
        }
	}
}
