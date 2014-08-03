using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Configly.Store;
using Configly.Store.Entities;

namespace Configly.TestClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Any Key To Connect To Server:");
            string application = Console.ReadLine();

            var config = new SettingsStoreConfiguration
            {
                ServerUrl = ConfigurationManager.AppSettings["SettingsServerUrl"]
            };

            if (args.Length > 0) config.Scope = args[0];

            ILogger tracking = new TraceLogger();

            if (application == "1")
            {
                var store = new SettingsStore<AppSettings>(config, tracking);
                store.OnRefresh += store_OnRefresh;

                WriteSettingsToConsole("Initial", store.Get());

            }
            else
            {
                var store = new SettingsStore<TestSettings>(config, tracking);
                store.OnRefresh += store_OnRefresh;

                WriteSettingsToConsole("Initial", store.Get());
            }
            Console.ReadLine();
        }



        static void store_OnRefresh(object sender, SettingsRefreshArgs e)
        {
            WriteSettingsToConsole("Refreshed", e.Settings);
        }

        private static void WriteSettingsToConsole(string type, object settings)
        {
            var json = JsonConvert.SerializeObject(settings);

            Console.WriteLine();
            Console.WriteLine(settings.GetType().Name);
            Console.WriteLine(type + " Settings:");
            Console.WriteLine(json);
        }
    }
}
