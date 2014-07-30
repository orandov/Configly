using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace TestClientNuget
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = new SettingsStoreConfiguration
            //{
            //    ServerUrl = ConfigurationManager.AppSettings["SettingsServerUrl"]
            //};

            //if (args.Length > 0) config.Scope = args[0];

            //string trackingUrl = ConfigurationManager.AppSettings["TrackingUrl"];
            //ILogger tracking = new TrackingLogger(trackingUrl, "Configly.TestClientApp");


            //var store = new SettingsStore<TestSettings>(config, tracking);
            //store.OnRefresh += store_OnRefresh;

            //Console.WriteLine("Retrieving settings from store...");
            //WriteSettingsToConsole("Initial", store.Get());
            //Console.ReadLine();
        }

        //static void store_OnRefresh(object sender, SettingsRefreshArgs e)
        //{
        //    WriteSettingsToConsole("Refreshed", e.Settings);
        //}

        //private static void WriteSettingsToConsole(string type, object settings)
        //{
        //    var json = JsonConvert.SerializeObject(settings);

        //    Console.WriteLine();
        //    Console.WriteLine(settings.GetType().Name);
        //    Console.WriteLine(type + " Settings:");
        //    Console.WriteLine(json);
        //}
    }
}
