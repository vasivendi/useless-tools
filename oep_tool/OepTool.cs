using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace OEP
{
    class OEPTool
    {
        FileSystemWatcher watcher;
        readonly string folder, oepfolder, zipname;
        public OEPTool(string directory)
        {
            folder = directory;
            oepfolder = directory + Path.DirectorySeparatorChar + ".oep";
            string dir_name = directory.Split(Path.DirectorySeparatorChar).Last();
            zipname = $"{dir_name}.zip";
            System.Console.WriteLine("zipname " + zipname);
            watcher = new FileSystemWatcher(directory);
            watcher.Created += OnUpdate;
            watcher.Changed += OnUpdate;
            watcher.Deleted += OnUpdate;
            watcher.Renamed += OnRename;
            watcher.Filter = "*.cs";
            watcher.IncludeSubdirectories = false;
            CopyAllCs();
            UpdateZip();
        }
        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            Log.Write("Watcher initialised");
        }
        void CopyAllCs()
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                if (!file.EndsWith(".cs"))
                {
                    continue;
                }
                EnvTool.CopyToOep(file, oepfolder);
            }
            Log.Write("Copied files to oep before watching");
        }
        void OnUpdate(object sender, FileSystemEventArgs arg)
        {
            // *.cs filter is in place, but to be double sure
            if (arg.Name == zipname)
            {
                return;
            }
            Log.Write($"{Enum.GetName(typeof(WatcherChangeTypes), arg.ChangeType)} => {arg.Name}");
            switch (arg.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    EnvTool.CopyToOep(arg.FullPath, oepfolder);
                    break;
                case WatcherChangeTypes.Changed:
                    EnvTool.DeleteFromOep(arg.FullPath, oepfolder);
                    EnvTool.CopyToOep(arg.FullPath, oepfolder);
                    break;
                case WatcherChangeTypes.Deleted:
                    EnvTool.DeleteFromOep(arg.FullPath, oepfolder);
                    break;

            }
            UpdateZip();
        }
        void OnRename(object sender, RenamedEventArgs arg){
            Log.Write($"{Enum.GetName(typeof(WatcherChangeTypes), arg.ChangeType)} => {arg.Name}");
            EnvTool.DeleteFromOep(arg.OldFullPath, oepfolder);
            EnvTool.CopyToOep(arg.FullPath, oepfolder);
            UpdateZip();
        }
        void UpdateZip()
        {
            //Log.Write("Creating zip");
            try{
                File.Delete(zipname);
            }
            catch (Exception){
                
            }
            ZipFile.CreateFromDirectory(oepfolder, zipname);
            Log.Write("Zip updated");
        }
    }
}
