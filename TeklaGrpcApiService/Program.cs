namespace TeklaGrpcApiService
{
    using CommandLine;

    using Google.Protobuf.WellKnownTypes;

    using Grpc.Core;

    using TeklaGrpcApiService.Services;

    using Microsoft.Extensions.Logging;

    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using TeklaService;
    using Tekla.Structures.Model;

    class Program
    {

        static int RunAndReturnExitCode(Options options)
        {


            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                }));


            ILogger logger = loggerFactory.CreateLogger("TeklaGrpcApiService");
            Trace.Listeners.Add(new TextWriterTraceListener(options.Log));
            Trace.AutoFlush = true; // Ensure each write is flushed immediately

            if (File.Exists(options.Log))
            {
                File.Delete(options.Log);
            }

            if (options.TestApiConnection)
            {
                try
                {
                    var modelInfo = new Model().GetInfo();
                    logger.LogInformation($"{modelInfo.ModelName} : {modelInfo.ModelPath}");
                    return 0;
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"Failed to connect to TS: {ex.Message}");
                    return 1;
                }
            }


            logger.LogInformation($"Tekla Api Server Requested to start with Port : {options.Port} => Log: {options.Log}");

            var apiService = new TeklaGrpcApiService(logger);

            Server server = new()
            {
                Services = {
                    TeklaServiceApi.BindService(apiService)
                },
                Ports = { new ServerPort("localhost", options.Port, ServerCredentials.Insecure) }
            };
            apiService.Server = server;
            server.Start();
            logger.LogInformation($"server listening on port {options.Port}");

            // Block the main thread until the server is requested to shut down
            WaitForShutdownAsync(server).Wait();

            return 0;
        }

        private static async System.Threading.Tasks.Task WaitForShutdownAsync(Server server)
        {
            var shutdownTask = server.ShutdownTask;
            await shutdownTask;
            Trace.WriteLine("Server has been stopped.");
        }

        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(options => RunAndReturnExitCode(options), _ => 1);
        }
    }
}
