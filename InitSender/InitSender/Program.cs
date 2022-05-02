using System;
using HidSharp;
using System.Linq;


namespace InitSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HidDevice tablet = Array.Find(DeviceList.Local.GetHidDevices().ToArray(),
                delegate (HidDevice d)
                {
                    return d.ProductID == 187 && d.VendorID == 1386 && d.MaxFeatureReportLength > 0;
                }
            );

            if (tablet == null)
            {
                Console.WriteLine("Failed to find PTK-1240");
                return;
            }

            HidStream hidStream;
            if (tablet.TryOpen(out hidStream))
            {
                Console.WriteLine("Opened device.");
                string[] features = {
                    "IAQKKBoAAAAA",
                    "IAUKKBoAAAAA",
                    "IAYKKBoAAAAA",
                    "IAcKKBoAAAAA"
                };
                int i = 0;
                while(true)
                {
                    if (i >= features.Length)
                        i = 0;
                    var feature = Convert.FromBase64String(features[i]);
                    hidStream.SetFeature(feature);          
                    System.Threading.Thread.Sleep(20);
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Failed to open device.");
            }
        }
    }
}
