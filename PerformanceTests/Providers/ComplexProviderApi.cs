using System.Collections.Generic;

namespace Nanoray.Pintail.PerformanceTests.Providers
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

    public class ComplexProviderApi : SimpleProviderApi
    {
        public string GetName(IProxiedInputA inputA) => inputA.name;
        public string GetName(IProxiedInputB inputB) => inputB.name;

        public IList<IProxiedInputA> list { get; set; } = new List<IProxiedInputA>();
    }
}
