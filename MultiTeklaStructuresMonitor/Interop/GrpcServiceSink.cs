namespace MultiTeklaStructuresMonitor.Interop
{
    using Microsoft.Win32;

    using MultiTeklaStructuresMonitor.Helpers;

    public class GrpcServiceSink
    {
        private static readonly string CurrentRunningPath = Directory.GetParent(typeof(DriverStarter).Assembly.Location)!.FullName;
        private static readonly List<GrpcServiceClient> clients = new List<GrpcServiceClient>();

        private  GrpcServiceSink()
        {

        }

        public static GrpcServiceSink CreateInstance(ILogger logger)
        {
            // read all registries for installations path from Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Trimble\Tekla Structures\VERSION\setup key MainDir and Version where VERSION can be anything
            var installDirData = RegistryHelpers.ReadInstalledApplications(logger);

            // start a grpc server for each installation path
            foreach (var installDir in installDirData)
            {
                var client = DriverStarter.StartDriverAndServer(installDir, logger);

                if (client != null)
                {
                    clients.Add(client);
                }
            }

            return new GrpcServiceSink();
        }

        public List<string> GetAllOpenModels()
        {
            var openModels = new List<string>();
            foreach (var client in clients)
            {
                var openModelReply = client.GetOpenModel("GetOpenModel");
                openModels.Add($"Client: {client.InstallData.TSVersionDir} : {client.InstallData.ProductVersion} : Status: {openModelReply}");
            }

            return openModels;
        }
    }
}
