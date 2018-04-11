using System;
using System.Management;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCard1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(@"Select * FROM Win32_UsbHub"))
                {
                    foreach (var device in searcher.Get())
                    {
                        var properties = device.Properties;
                        foreach (var property in properties)
                        {
                            var value = property.Value is string[] array ? string.Join(", ", array) : property.Value;
                            Console.WriteLine($"property is {property.Name} value is {value}");
                        }
                        var Device_ID = (string)device.GetPropertyValue("DeviceID");
                        Console.WriteLine("===================== end of device =======================");
                    }
                }

                Console.WriteLine("Enumerating smartcard readers");
                var smartcardGuid = "{50dd5230-ba8a-11d1-bf5d-0000f805f530}";
                using (var searcher = new ManagementObjectSearcher($@"Select * FROM Win32_PnPEntity where ClassGuid='{smartcardGuid}'"))
                {
                    foreach (var device in searcher.Get())
                    {
                        var properties = device.Properties;
                        foreach (var property in properties)
                        {
                            var value = property.Value is string[] array ? string.Join(", ", array) : property.Value;
                            Console.WriteLine($"property is {property.Name} value is {value}");
                        }
                        Console.WriteLine("---- end of smart card reader");
                    }
                }
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
