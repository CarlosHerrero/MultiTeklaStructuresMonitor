namespace MultiTeklaStructuresMonitor.Helpers
{
    using Microsoft.Win32;
    using MultiTeklaStructuresMonitor.Interop;

#pragma warning disable CA1416 // yes yes, this is to be used on on windows
    internal static class RegistryHelpers
    {
        internal static List<InstallDirData> ReadInstalledApplications(ILogger logger)
        {
            string baseRegistryPath = @"SOFTWARE\Trimble\Tekla Structures";
            var installDirData = new List<InstallDirData>();

            using (RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(baseRegistryPath)!)
            {
                if (baseKey == null)
                {
                    logger.LogInformation("No Tekla Structures are installed in the machine");
                    return installDirData;
                }

                // Get all subkey names (representing versions)
                string[] versionSubKeys = baseKey.GetSubKeyNames();

                foreach (string version in versionSubKeys)
                {
                    // Build the setup key path for each version
                    string setupKeyPath = $@"{baseRegistryPath}\{version}\setup";

                    // Open the setup key
                    using (RegistryKey setupKey = Registry.LocalMachine.OpenSubKey(setupKeyPath)!)
                    {
                        if (setupKey != null)
                        {
                            // Read the MainDir and Version values
                            string? mainDir = setupKey.GetValue("MainDir") as string;
                            string? versionValue = setupKey.GetValue("TSVersionDir") as string;
                            string? productVersion = setupKey.GetValue("ProductVersion") as string;

                            installDirData.Add(new InstallDirData(versionValue!, mainDir!, productVersion!));

                            // Print the results
                            logger.LogInformation($"Found  a installed Version MainDir: {mainDir} {versionValue} {productVersion}");
                        }
                    }
                }
            }

            return installDirData;
        }
    }
}
