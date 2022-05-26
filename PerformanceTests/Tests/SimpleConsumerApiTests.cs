using System.Reflection;
using System.Reflection.Emit;
using Nanoray.Pintail.PerformanceTests.Consumers;
using Nanoray.Pintail.PerformanceTests.Providers;
using NBench;

namespace Nanoray.Pintail.PerformanceTests.Tests
{
    internal class SimpleConsumerApiTests
    {
        private Counter _counter;

        private ProxyManager<Nothing> CreateProxyManager(ProxyManagerConfiguration<Nothing>? configuration = null)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName($"Nanoray.Pintail.Proxies, Version={this.GetType().Assembly.GetName().Version}, Culture=neutral"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"Proxies");
            var manager = new ProxyManager<Nothing>(moduleBuilder, configuration);
            return manager;
        }

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            this._counter = context.GetCounter("SimpleApi");
        }

        [PerfBenchmark(NumberOfIterations = 3, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterMeasurement("SimpleApi")]
        [GcThroughputAssertion(GcMetric.TotalCollections, GcGeneration.AllGc, MustBe.LessThanOrEqualTo, 10.0)]
        public void TestSuccessfulBasicApi()
        {
            var manager = this.CreateProxyManager();
            var providerApi = new SimpleProviderApi();

            var consumerApi = manager.ObtainProxy<ISimpleConsumerApi>(providerApi)!;

            consumerApi.VoidMethod();
            _ = consumerApi.IntMethod(123);
            _ = consumerApi.DefaultMethod(12);
            _ = consumerApi.IntProperty;
            _ = consumerApi["asdf"];
            _ = consumerApi.MapperMethod("word.", (t) => t.Length);
            _counter.Increment();
        }
    }
}
