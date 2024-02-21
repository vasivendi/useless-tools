using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OEP
{
    static class EnvTool
    {
        public static bool HasCsproj(string folder)
        {
            return Directory.Exists(folder) && Directory.GetFiles(folder).Any(x => x.EndsWith(".csproj"));
        }
        public static bool HasOEPInstalled(string folder)
        {
            return Directory.Exists(folder) && Directory.GetDirectories(folder).Any(x => x.EndsWith(Path.DirectorySeparatorChar + ".oep"));
        }
        public static void SetupOep(string folder)
        {
            string OEP = folder + Path.DirectorySeparatorChar + ".oep";
            Directory.CreateDirectory(OEP);
            string[] files = Directory.GetFiles(folder);
            CopyIfExists(files, ".sln", OEP);
            if (CopyIfExists(files, ".csproj", OEP))
            {
                string hostVersion = CsprojString.ToCsprojReady();
                string csprojfile = GetIfExists(files,".csproj") ?? throw new Exception("File deleted after finding it");
                Log.Write("Host has version "+hostVersion);
                (string before, string version, string after) = CsprojString.Separate(File.ReadAllText(csprojfile));
                Log.Write("Csproj has version "+version);
                if (version == hostVersion){
                    Log.Write("No variance, skipping override...");
                    return;
                }
                Log.Write("Changing csproj target to host version");
                Console.WriteLine(before+hostVersion+after);
                File.WriteAllText(csprojfile,before+hostVersion+after);
            }
        }
        static bool CopyIfExists(string[] files, string endsWith, string oep)
        {
            string? file = GetIfExists(files, endsWith);
            if (file == null)
            {
                return false;
            }
            CopyToOep(file, oep);
            return true;
        }

        static string? GetIfExists(string[] files, string endsWith)
        {
            foreach (string f in files)
            {
                if (f.EndsWith(endsWith))
                {
                    return f;
                }
            }
            return null;
        }
        public static bool CopyToOep(string file_full, string oep_folder)
        {
            string fileName = Path.GetFileName(file_full);
            try
            {
                File.Copy(file_full, oep_folder + Path.DirectorySeparatorChar + fileName);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static bool DeleteFromOep(string file_full, string oep_folder){
            string fileName = Path.GetFileName(file_full);
            try
            {
                File.Delete(oep_folder + Path.DirectorySeparatorChar + fileName);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

    }
    static class Log
    {
        public static void Write(string value)
        {
            Console.WriteLine($"[OEP] {value}");
        }
    }
}

