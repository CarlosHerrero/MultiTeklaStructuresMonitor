namespace MultiTeklaStructuresMonitor.Interop
{
    using Grpc.Net.Client;

   
    using TeklaService;

    public class GrpcServiceClient
    {
        private readonly TeklaServiceApi.TeklaServiceApiClient client;
        
        public GrpcServiceClient(InstallDirData dirData, int pidOfRunningProcess, int port)
        {
            var grpcServerUrl = $"http://localhost:{port}"; // Adjust the URL and port as needed
            var channel = GrpcChannel.ForAddress(grpcServerUrl);
            client = new TeklaServiceApi.TeklaServiceApiClient(channel);
            this.InstallData = dirData;
        }

        public InstallDirData InstallData { get; private set; }

        public string Ping(string name)
        {
            var request = new StringRequest { Name = name };
            var reply = client.Ping(request);
            return reply.Message;
        }

        public string GetOpenModel(string name)
        {
            var request = new StringRequest { Name = name };
            var data = client.GetOpenModel(request);
            return data.Message;
        }
    }
}
