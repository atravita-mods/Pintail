using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nanoray.Pintail.PerformanceTests.Providers
{
    public delegate void CustomGenericOutDelegate<T>(out T param);

    public enum StateEnum
    {
        State0, State1, State2
    }

    public interface IApiResult
    {
        public string Text { get; }
    }

    public class SimpleProviderApi
    {
        protected Func<IApiResult, IApiResult> Mapper = (r) => r;
        protected CustomGenericOutDelegate<StateEnum> CustomOutDelegate = (out StateEnum p) => p = StateEnum.State0;

        public void VoidMethod() { }

        public int IntMethod(int num)
            => num;

        public void OutIntMethod(out int num)
        {
            num = 1;
        }

        public void OutObjectMethod(out object? obj)
        {
            obj = new StringBuilder();
        }

        public void RefIntMethod(ref int num)
        {
            num = 1;
        }

        public void RefObjectMethod(ref object? obj)
        {
            obj = new StringBuilder();
        }

        public int DefaultMethod(int num) => num
            ;

        public int IntProperty
            => 42;

        public string this[string key]
            => key;

        public R MapperMethod<T, R>(T t, Func<T, R> mapper)
            => mapper(t);

        //public string? IsAssignableTest(object? anyObj)
        //    => anyObj?.ToString();
    }
}
