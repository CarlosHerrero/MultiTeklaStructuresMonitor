namespace TeklaGrpcApiService.Services
{
    using Grpc.Core;

    using Microsoft.Extensions.Logging;

    using System.Diagnostics;

    using Tekla.Structures.Model;

    using TeklaService;

    public class TeklaGrpcApiService : TeklaServiceApi.TeklaServiceApiBase
    {
        public TeklaGrpcApiService()
        {
        }

        public Server? Server { get; set; }

        public override Task<StringReply> Ping(StringRequest request, ServerCallContext context)
        {
            Trace.WriteLine($"Ping:  {request.Name}");
            return System.Threading.Tasks.Task.FromResult(new StringReply { Message = $"Hello, {request.Name} im node: {Process.GetCurrentProcess().Id}" });
        }

        public override Task<StringReply> GetOpenModel(StringRequest request, ServerCallContext context)
        {
            Trace.WriteLine($"GetOpenModel:  {request.Name}");
            try
            {
                var modelInfo = new Model().GetInfo();
                Trace.WriteLine($"GetOpenModel Ok:  {modelInfo.ModelName} : {modelInfo.ModelPath}");
                return System.Threading.Tasks.Task.FromResult(new StringReply { Message = $"{modelInfo.ModelName} : {modelInfo.ModelPath}" });
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"GetOpenModel Failed:  {ex.Message}");
                return System.Threading.Tasks.Task.FromResult(new StringReply { Message = $"Tekla Structures not running" });
            }
        }

        public override async Task<StringReply> StopServer(StringRequest request, ServerCallContext context)
        {
            Trace.WriteLine("Shutdown request received.");
            if (Server != null)
            {
                await Server.ShutdownAsync();
                return new StringReply { Message = "Server is shutting down." };
            }

            Trace.WriteLine("Shutdown not handled, Server not configued correctly");
            return new StringReply { Message = "Server cannot be shut down." };
        }
    }
}
