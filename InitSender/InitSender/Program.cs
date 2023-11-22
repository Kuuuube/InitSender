using System;
using HidSharp;
using System.Linq;
using System.IO;

namespace InitSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string tablet_model;
            string init_file;
            int tablet_vid = 1386;
            int tablet_pid = 0;
            int init_delay = 0;
            int auto_repeat = 1;
            int init_type = 1;

            if (args.Length != 0)
            {
                if (args[0] == "--help" || args[0] == "-help" || args[0] == "-h" || args[0] == "--h") {
                    Console.WriteLine("# InitSender\nAccepts newline-separated, base64-encoded feature inits or output inits.\nRun either with or without command line args.\n## Command line args\n```\nInitSender {tablet model} {init file} {init delay} {auto repeat} {init type} {tablet vid} {tablet pid}\n```\n### Tablet Model\n`0`: Custom\n`1`: PTK-540WL\n`2`: PTK-640\n`3`: PTK-840\n`4`: PTK-1240\n### Init File\nRelative or absolute path to the file containing newline-separated, base64-encoded feature inits or output inits.\n### Init Delay\nInteger to use for delay between inits in miliseconds. (Use 0 for smoothest animations.)\n### Auto Repeat\nWhether or not to loop through the init file.\n`1`: Auto repeat\n`2`: Do not auto repeat\n### Init Type\nType of init to send. For Wacom tablets you likely want Feature Init. For Xencelabs tablets you likely want Output Init.\n`1`: Feature Init\n`2`: Output Init\n### Tablet VID\nBase10 integer containing the Vendor ID for tablet. Only used when Tablet Model = 0.\n### Tablet PID\nBase10 integer containing the Product ID for tablet. Only used when Tablet Model = 0.");
                    return;
                }
                tablet_model = args[0];
                init_file = args[1];
                init_delay = Int16.Parse(args[2]);
                auto_repeat = Int16.Parse(args[3]);
                init_type = Int16.Parse(args[4]);
                switch (tablet_model)
                {
                    case "1": //PTK-540WL
                        tablet_pid = 188;
                        break;
                    case "2": //PTK-640
                        tablet_pid = 185;
                        break;
                    case "3": //PTK-840
                        tablet_pid = 186;
                        break;
                    case "4": //PTK-1240
                        tablet_pid = 187;
                        break;
                    default:
                        tablet_vid = Int16.Parse(args[5]);
                        tablet_pid = Int16.Parse(args[6]);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Send inits to which tablet?\n0. Custom\n1. PTK-540WL\n2. PTK-640\n3. PTK-840\n4. PTK-1240");
                tablet_model = Console.ReadLine();

                switch (tablet_model)
                {
                    case "1": //PTK-540WL
                        tablet_pid = 188;
                        break;
                    case "2": //PTK-640
                        tablet_pid = 185;
                        break;
                    case "3": //PTK-840
                        tablet_pid = 186;
                        break;
                    case "4": //PTK-1240
                        tablet_pid = 187;
                        break;
                    default:
                        Console.WriteLine("Set custom VID and PID (base10)\nVID:");
                        tablet_vid = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("PID:");
                        tablet_pid = Int16.Parse(Console.ReadLine());
                        break;
                }

                Console.WriteLine("File to use for inits");
                init_file = Console.ReadLine();
                Console.WriteLine("Init delay");
                init_delay = Int16.Parse(Console.ReadLine());
                Console.WriteLine("Auto Repeat?\n1. Auto repeat\n2. Do not auto repeat");
                auto_repeat = Int16.Parse(Console.ReadLine());
                Console.WriteLine("Init type\n1. Feature Init\n2. Output Init");
                init_type = Int16.Parse(Console.ReadLine());
            }

            string[] features = File.ReadLines(init_file).ToArray();
            for (int i = 0; i < features.Length; i++)
            {
                features[i] = features[i].Replace("\"", "");
                features[i] = features[i].Replace(",", "");
            }

            HidDevice tablet = Array.Find(DeviceList.Local.GetHidDevices().ToArray(),
                delegate (HidDevice d)
                {
                    if (init_type == 1) {
                        return d.ProductID == tablet_pid && d.VendorID == tablet_vid && d.MaxFeatureReportLength > 0;
                    }
                    return d.ProductID == tablet_pid && d.VendorID == tablet_vid && d.MaxOutputReportLength > 0;
                }
            );

            if (tablet == null)
            {
                Console.WriteLine("Failed to find tablet\nPID:" + tablet_pid + " VID:" + tablet_vid);
                return;
            }

            HidStream hidStream;
            if (tablet.TryOpen(out hidStream))
            {
                Console.WriteLine("Opened device.");
                int i = 0;
                bool has_repeated = false;
                while(auto_repeat == 1 || !has_repeated)
                {
                    if (i >= features.Length)
                    {
                        i = 0;
                        has_repeated = true;
                    }
                    var feature = Convert.FromBase64String(features[i]);
                    if (init_type == 1) {
                        hidStream.SetFeature(feature);
                    } else if (init_type == 2) {
                        hidStream.Write(feature);
                    }
                    System.Threading.Thread.Sleep(init_delay);
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
