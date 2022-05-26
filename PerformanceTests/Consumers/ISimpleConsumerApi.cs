using System;

namespace Nanoray.Pintail.PerformanceTests.Consumers
{
    public interface ISimpleConsumerApi
    {
        void VoidMethod();
        int IntMethod(int num);
        void OutIntMethod(out int num);
        void OutObjectMethod(out object? obj);
        void RefIntMethod(ref int num);
        void RefObjectMethod(ref object? obj);
        int DefaultMethod(int num);
        int IntProperty { get; }
        string this[string key] { get; }
        R MapperMethod<T, R>(T t, Func<T, R> mapper);
        //object? IsAssignableTest(string? anyObj);
    }
}
