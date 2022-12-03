using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nanoray.Pintail.Tests.Private.Provider;
public class EnumeratorProvider
{
    public IGenericTwo<StringBuilder> GetSimpleGeneric() => new GenericTwoImpl<StringBuilder>() { Value = new("HIIIII") };

    public IGenericCollection<IGenericTwo<StringBuilder>> GetOne()
    {
        return new GenericCollectionImpl<IGenericTwo<StringBuilder>>()
        {
            { "test", new GenericTwoImpl<StringBuilder>(){ Value = new StringBuilder("HIIIII") } },
        };
    }

    public IGenericCollection<IList<StringBuilder>> GetUnproxied()
    {
        return new GenericCollectionImpl<IList<StringBuilder>>()
        {
            { "test", new List<StringBuilder>() { new StringBuilder("HIIIII") } }
        };
    }
}

public interface IGenericTwo<TValue>
{
    public TValue? Value { get; }
}

public interface IGenericCollection<TValue> : IReadOnlyDictionary<string, TValue>
{
    public IReadOnlyDictionary<string, TValue> CalculatedValues { get; }
}

public class GenericTwoImpl<TValue> : IGenericTwo<TValue>
{
    public TValue? Value { get; internal set; } = default;
}

public class GenericCollectionImpl<TValue> : IGenericCollection<TValue>
{
    private readonly Dictionary<string, TValue> _values = new();

    internal void Add(string key, TValue value) => this._values[key] = value;

    public TValue this[string key] => this._values[key];

    public IReadOnlyDictionary<string, TValue> CalculatedValues => this._values;
    public IEnumerable<string> Keys => this._values.Keys;
    public IEnumerable<TValue> Values => this._values.Values;
    public int Count => this._values.Count;

    public bool ContainsKey(string key) => this._values.ContainsKey(key);
    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator() => this._values.GetEnumerator();
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value) => this._values.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => this._values.GetEnumerator();
}
