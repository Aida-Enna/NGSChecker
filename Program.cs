/*
Humans must learn to apply their intelligence correctly and evolve beyond their current state.
People must change. Otherwise, even if humanity expands into space, it will only create new
conflicts, and that would be a very sad thing. - Aeolia Schenberg, 2091 A.D.
*/
using Microsoft.Win32;
using System;
using System.Management;
using System.Reflection;

namespace NGSChecker
{
    internal class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern long GetEnabledXStateFeatures();

        private static void Main()
        {
            try
            {
                bool Is64bit = System.Environment.Is64BitOperatingSystem;
                bool HasAVXSupport = CheckForAVX();
                bool IssueFound = false;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("NGS Checker v" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
                Console.WriteLine("This will check to make sure your system will work after the Graphics Update for PSO2.");
                Console.Write("The source code for this program can be found at ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("https://github.com/Aida-Enna/NGSChecker");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(".");
                string ProcessorName = "Unknown??";
                try
                {
                    ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                    foreach (ManagementObject mo in mos.Get())
                    {
                        ProcessorName = mo["Name"].ToString();
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get the proper name of the processor. Is Windows corrupted somehow? This issue may prevent the game from working. If you manage to fix it, please let me know via github or discord (Aida Enna#0001).");
                    Console.ForegroundColor = ConsoleColor.White;
                    ProcessorName = "Unknown (Failed to retrieve)";
                }
                Console.WriteLine("Processor Name: " + ProcessorName);
                Console.WriteLine("64-bit OS: " + Is64bit);
                if (!Is64bit)
                {
                    //Check the registry to see what the processor calls itself
                    var rk = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
                    if (rk.GetValue("Identifier").ToString().IndexOf("64") > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Your OS is not 64-bit (" + rk.GetValue("Identifier").ToString() + "). Your CPU appears to support 64-bit, so you must reinstall Windows as a 64-bit OS.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Your OS is not 64-bit (" + rk.GetValue("Identifier").ToString() + "). Your CPU also does not appear to support 64-bit. You may need to purchase a newer CPU.");
                    }
                    IssueFound = true;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("AVX Support: " + HasAVXSupport);
                if (!HasAVXSupport)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your CPU does not support the AVX feature. This means it is too old to play PSO2 after the graphics update. You will need to purchase a newer CPU.");
                    IssueFound = true;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (!IssueFound)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("No issues were found when checking to make sure your system will run PSO2 after the graphics update. Awesome!");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch(Exception WhatBroke)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong! Please create an issue on the github website so I can fix it. Error info:" + Environment.NewLine + WhatBroke.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static bool CheckForAVX()
        {
            try
            {
                return (GetEnabledXStateFeatures() & 4) != 0;
            }
            catch
            {
                return false;
            }
        }
    }
}