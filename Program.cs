using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace UPay
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new UPayService() 
            };

            if(Environment.UserInteractive)
            {
                RunServicesInInteractiveMode(servicesToRun);
            }
            else
            {
                ServiceBase.Run(servicesToRun);
            }
        }

        static void RunServicesInInteractiveMode(ServiceBase[] servicesToRun)
        {
            StartServices(servicesToRun);

            Console.WriteLine("Running in interactive mode...");
            var @continue = true;
            while (@continue)
            {
                var keyInfo = Console.ReadKey();

                if(keyInfo.Key == ConsoleKey.Q && keyInfo.Modifiers == (ConsoleModifiers.Control | ConsoleModifiers.Shift))
                {
                    @continue = false;
                }
            }

            StopServices(servicesToRun);
        }

        private static void StartServices(ServiceBase[] servicesToRun)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
            var onStartMethod = typeof(ServiceBase).GetMethod("OnStart", bindingFlags);

            foreach(var service in servicesToRun)
            {
                onStartMethod.Invoke(service, new object[] { new string[] { } });
            }
        }

        private static void StopServices(ServiceBase[] servicesToRun)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
            var onStopMethod = typeof(ServiceBase).GetMethod("OnStop", bindingFlags);

            foreach (var service in servicesToRun)
            {
                onStopMethod.Invoke(service, null);
            }
        }
    }
}
