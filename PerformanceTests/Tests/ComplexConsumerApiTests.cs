using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Nanoray.Pintail.PerformanceTests.Consumers;
using Nanoray.Pintail.PerformanceTests.Providers;
using NBench;

namespace Nanoray.Pintail.PerformanceTests.Tests
{
    internal class ComplexConsumerApiTests
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
            this._counter = context.GetCounter("ComplexApi");
        }

        [PerfBenchmark(NumberOfIterations = 3, RunMode = RunMode.Throughput, RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [CounterMeasurement("ComplexApi")]
        [GcThroughputAssertion(GcMetric.TotalCollections, GcGeneration.AllGc, MustBe.LessThanOrEqualTo, 10.0)]
        public void TestSuccessfulComplexApi()
        {
            var manager = this.CreateProxyManager();
            var providerApi = new ComplexProviderApi();

            var consumerApi = manager.ObtainProxy <IComplexConsumerApi>(providerApi)!;

            consumerApi.VoidMethod();
            _ = consumerApi.IntMethod(123);
            _ = consumerApi.DefaultMethod(12);
            _ = consumerApi.IntProperty;
            _ = consumerApi["asdf"];
            _ = consumerApi.MapperMethod("word.", (t) => t.Length);

            _ = consumerApi.GetName(new ProxiedInputB());
            _ = consumerApi.GetName(new ProxiedInputA());
            consumerApi.list = new List<Consumers.IProxiedInputA>();
            this._counter.Increment();
        }
    }
}
