namespace MultiTeklaStructuresMonitor.Interop
{
    public class InstallDirData
    {
        public InstallDirData(string version, string installDir, string productVersion)
        {
            TSVersionDir = version;
            MainDir = installDir;
            ProductVersion = productVersion;
        }

        public string TSVersionDir { get; private set; }
        public string MainDir { get; private set; }

        public string ProductVersion { get; private set; }
    }
}