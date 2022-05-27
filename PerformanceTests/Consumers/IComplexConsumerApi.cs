using System.Collections.Generic;

namespace Nanoray.Pintail.PerformanceTests.Consumers
{
    public interface IProxiedInputA
    {
        public string name { get; set; }
        public int value { get; set; }
    }

    public interface IProxiedInputB
    {
        public string name { get; set; }
        public int othervalue { get; set; }
    }

    public class ProxiedInputA: IProxiedInputA
    {
        public string name { get; set; } = "A";
        public int value { get; set; } = 5;
    }

    public class ProxiedInputB: IProxiedInputB
    {
        public string name { get; set; } = "B";
        public int othervalue { get; set; } = 3;
    }

    public interface IComplexConsumerApi: ISimpleConsumerApi
    {
        public string GetName(IProxiedInputA inputA) => inputA.name;
        public string GetName(IProxiedInputB inputB) => inputB.name;

        public IList<IProxiedInputA> list { get; set; }
    }
}
