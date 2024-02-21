using System;
using System.IO;
using System.Linq;

namespace OEP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Contains("--help"))
            {
                Console.WriteLine("No args is normal launch\n--remove -> delete oep folder and exit\n--wipe -> delete oep folder and start");
                return;
            }
            if (args.Contains("--remove"))
            {
                string run_loc = Directory.GetCurrentDirectory();
                if (EnvTool.HasOEPInstalled(run_loc))
                {
                    DirectoryInfo info = new DirectoryInfo(
                        run_loc + Path.DirectorySeparatorChar + ".oep"
                    );
                    info.Delete(true);
                    Log.Write("Removed OEP folder");
                }
                else
                {
                    Log.Write("OEP is not installed in this directory");
                }
                return;
            }
            if (args.Contains("--wipe"))
            {
                string run_loc = Directory.GetCurrentDirectory();
                if (EnvTool.HasOEPInstalled(run_loc))
                {
                    DirectoryInfo info = new DirectoryInfo(
                        run_loc + Path.DirectorySeparatorChar + ".oep"
                    );
                    info.Delete(true);
                    Log.Write("Wiped OEP folder, starting up");
                }
                else
                {
                    Log.Write("OEP is not installed in this directory, starting up");
                }
            }
            Run();
            Log.Write("Exiting");
        }
        public static void Run()
        {
            string run_loc = Directory.GetCurrentDirectory();
            if (!EnvTool.HasCsproj(run_loc))
            {
                Log.Write("I don't think this is a csharp project");
                return;
            }
            if (!EnvTool.HasOEPInstalled(run_loc))
            {
                Log.Write("Setting up required folder");
                EnvTool.SetupOep(run_loc);
            }
            OEPTool tool = new OEPTool(run_loc);
            tool.Start();
            Log.Write("Press escape to exit");
            while (Console.ReadKey().Key != ConsoleKey.Escape) { }
        }
    }
}
