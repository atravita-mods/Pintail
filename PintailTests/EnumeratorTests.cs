using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Nanoray.Pintail.Tests.Private.Consumer;
using Nanoray.Pintail.Tests.Private.Provider;

using NUnit.Framework;

namespace Nanoray.Pintail.Tests;

[TestFixture]
public class EnumeratorTests
{
    private ProxyManager<Nothing> CreateProxyManager(ProxyManagerConfiguration<Nothing>? configuration = null)
    {
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName($"Nanoray.Pintail.Proxies, Version={this.GetType().Assembly.GetName().Version}, Culture=neutral"), AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule($"Proxies");
        var manager = new ProxyManager<Nothing>(moduleBuilder, configuration);
        return manager;
    }

    [Test]
    public void MildScreaming()
    {
        var manager = this.CreateProxyManager();
        var provider = new EnumeratorProvider();
        var consumer = manager.ObtainProxy<IEnumeratorConsumer>(provider);

        var simple = consumer.GetSimpleGeneric();
        Assert.AreEqual(simple.Value!.GetType(), typeof(StringBuilder));

        Private.Consumer.IGenericCollection<IList<StringBuilder>>? unproxied = consumer.GetUnproxied();

        IEnumerator<KeyValuePair<string, IList<StringBuilder>>>? unproxiedEnumerator = unproxied.GetEnumerator();
        Assert.DoesNotThrow(() => unproxiedEnumerator.MoveNext());
        Assert.DoesNotThrow(() => _ = unproxiedEnumerator.Current);

        Private.Consumer.IGenericCollection<Private.Consumer.IGenericTwo<StringBuilder>>? proxied = consumer.GetOne();
        IEnumerator<KeyValuePair<string, Private.Consumer.IGenericTwo<StringBuilder>>>? proxiedEnumerator = proxied.GetEnumerator();

        Assert.DoesNotThrow(() => proxiedEnumerator.MoveNext());

        foreach (var prop in proxiedEnumerator.GetType().GetRuntimeProperties())
        {
            Console.WriteLine(prop.PropertyType.ToString());
        }
        Assert.DoesNotThrow(() => _ = proxiedEnumerator.Current);
    }
}
