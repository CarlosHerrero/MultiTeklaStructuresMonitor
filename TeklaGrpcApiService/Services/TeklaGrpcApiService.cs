namespace TeklaGrpcApiService.Services
{
    using Grpc.Core;

    using Microsoft.Extensions.Logging;

    using System.Diagnostics;

    using Tekla.Structures.Model;

    using TeklaService;

    public class TeklaGrpcApiService : TeklaServiceApi.TeklaServiceApiBase
    {
        private readonly ILogger logger;

        public TeklaGrpcApiService(ILogger logger)
        {
            this.logger = logger;
        }

        public Server? Server { get; set; }

        public override Task<StringReply> Ping(StringRequest request, ServerCallContext context)
        {
            logger.LogInformation($"Ping:  {request.Name}");
            return System.Threading.Tasks.Task.FromResult(new StringReply { Message = $"Hello, {request.Name} im node: {Process.GetCurrentProcess().Id}" });
        }

        public override Task<StringReply> GetOpenModel(StringRequest request, ServerCallContext context)
        {
            logger.LogInformation($"GetOpenModel:  {request.Name}");
            try
            {
                var modelInfo = new Model().GetInfo();
                return System.Threading.Tasks.Task.FromResult(new StringReply { Message = $"{modelInfo.ModelName} : {modelInfo.ModelPath}" });
            }
            catch (Exception)
            {
                return System.Threading.Tasks.Task.FromResult(new StringReply { Message = $"Tekla Structures not running" });
            }
        }

        public override async Task<StringReply> StopServer(StringRequest request, ServerCallContext context)
        {
            logger.LogInformation("Shutdown request received.");
            if (Server != null)
            {
                await Server.ShutdownAsync();
                return new StringReply { Message = "Server is shutting down." };
            }

            logger.LogInformation("Shutdown not handled, Server not configued correctly");
            return new StringReply { Message = "Server cannot be shut down." };
        }
    }
}
