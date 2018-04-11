// Copyright (c) Adrian Sims 2018
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System;
using System.Management;
using System.IO;

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
                        Console.WriteLine("---- end of smart card reader ----");
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
