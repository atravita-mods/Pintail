using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nanoray.Pintail.Tests.Private.Consumer;
public interface IEnumeratorConsumer
{
    IGenericTwo<StringBuilder> GetSimpleGeneric();

    IGenericCollection<IGenericTwo<StringBuilder>> GetOne();

    IGenericCollection<IList<StringBuilder>> GetUnproxied();

}

public interface IGenericTwo<TValue>
{
    public TValue? Value { get; }
}

public interface IGenericCollection<TValue> : IReadOnlyDictionary<string, TValue>
{
    public IReadOnlyDictionary<string, TValue> CalculatedValues { get; }
}
