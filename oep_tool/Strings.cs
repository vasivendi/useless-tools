namespace OEP
{
    static class CsprojString
    {
        public static (string, string, string) Separate(string fileContent)
        {
            string[] primary = fileContent.Split("<TargetFramework>");
            string[] secondary = primary[1].Split("</TargetFramework>");
            return (primary[0] + "<TargetFramework>", secondary[0], "</TargetFramework>"+secondary[1]);
        }
        public static string ToCsprojReady(){
            string raw = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            string longVersion = raw.Split(' ')[1];
            string[] versionSep = longVersion.Split('.');
            return "net"+versionSep[0]+'.'+versionSep[1];
        }
    }
}
