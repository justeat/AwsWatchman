using System;
using System.Threading.Tasks;
using Watchman.Engine.Generation;

namespace Watchman
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var startParams = CommandLineParser.ToParameters(args);
            if (startParams == null)
            {
                return ExitCode.InvalidParams;
            }

            return await GenerateAlarms(startParams);
        }

        private static async Task<int> GenerateAlarms(StartupParameters startParams)
        {
            try
            {
                var container = new IocBootstrapper().ConfigureContainer(startParams);
                var alarmGenerator = container.GetInstance<AlarmLoaderAndGenerator>();

                await alarmGenerator.LoadAndGenerateAlarms(startParams.RunMode);
                return ExitCode.Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Run failed: {ex.Message}");
                return ExitCode.RunFailed;
            }
        }
    }
}
