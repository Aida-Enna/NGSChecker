/*
Humans must learn to apply their intelligence correctly and evolve beyond their current state.
People must change. Otherwise, even if humanity expands into space, it will only create new
conflicts, and that would be a very sad thing. - Aeolia Schenberg, 2091 A.D.
　　　　 ,r‐､　　　　 　, -､
　 　 　 !　 ヽ　　 　 /　　}
　　　　 ヽ､ ,! -─‐- ､{　　ﾉ
　　　 　 ／｡　｡　　　 r`'､´
　　　　/ ,.-─- ､　　 ヽ､.ヽ　　　Haro
　　 　 !/　　　　ヽ､.＿, ﾆ|　　　　　Haro!
 　　　 {　　　 　  　 　 ,'
　　 　 ヽ　 　     　 ／,ｿ
　　　　　ヽ､.＿＿__r',／
*/
using System;

namespace NGSChecker
{
    internal class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern long GetEnabledXStateFeatures();

        private static void Main()
        {
            bool Is64bit = System.Environment.Is64BitOperatingSystem;
            bool HasAVXSupport = CheckForAVX();
            bool IssueFound = false;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("This will check to make sure your system will work after the Graphics Update for PSO2.");
            Console.Write("The source code for this program can be found at ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("https://github.com/Aida-Enna/NGSChecker");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("64-bit OS: " + Is64bit);
            if (!Is64bit)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Your OS is not 64 bits. Assuming your CPU supports it, you must reinstall Windows as a 64-bit OS.");
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