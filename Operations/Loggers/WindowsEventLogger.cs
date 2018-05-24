using System;
using System.Diagnostics;

namespace UPay.Operations
{
    public class WindowsEventLogger : ILogger
    {
        const string UPayEventLog = "Application";
        const string UPayEventLogSource = "UPayService";

        static WindowsEventLogger()
        {
            using (var log = new EventLog(UPayEventLog, "."))
            {
                log.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, log.MinimumRetentionDays);
            }
        }

        void ILogger.Error(Exception ex, object data)
        {
            Log(ExceptionExtensions.Traverse(ex), data, EventLogEntryType.Error);
        }

        void ILogger.Error(string message, object data)
        {
            Log(message, data, EventLogEntryType.Error);
        }

        void ILogger.Info(string message, object data)
        {
            Log(message, data, EventLogEntryType.Information);
        }
        
        void Log(string message, object data, EventLogEntryType entryType)
        {
            using (var log = new EventLog(UPayEventLog, ".", UPayEventLogSource))
            {
                if(data == null)
                {
                    log.WriteEntry(message, entryType);
                }
                else
                {
                    log.WriteEntry(message, entryType, eventID: 1001, category: 0);
                }
            }
        }
    }
}
