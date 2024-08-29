namespace TeklaGrpcApiService
{
    using CommandLine;

    class Options
    {
        [Option('p', Required = false, HelpText = "Port")]
        public int Port { get; set; }

        [Option('l', Required = false, HelpText = "LogFile")]
        public string Log { get; set; } = "";

        [Option('t', Required = false, HelpText = "LogFile")]
        public bool TestApiConnection { get; set; } = false;
    }
}