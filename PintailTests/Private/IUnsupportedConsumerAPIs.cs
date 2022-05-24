namespace Nanoray.Pintail.Tests.Consumer
{
    public enum ATooBigEnum { Just, A, Few, Too, Many, Values}

    public interface IInsufficientEnumValues
    {
        public ATooBigEnum method();
    }

    public interface IInvalidConsumerApi
    {
        public void NonExistentApiMethod();
    }

    public interface IInvalidNotMatchingArrayInput
    {
        public void NotMatchingArrayInput(int[] input);
    }

    public interface IInvalidNotMatchingEnumBackingField
    {
        public void NotMatchingEnumBackingType(StateEnum @enum);
    }

    public interface IInvalidIncorrectByRef
    {
        public void NotMatchingIncorrectByRef(int value);
    }

    public interface IRequiresBoxing
    {
        public object TestMethod(int value);
    }

    public interface IRequiresUnboxing
    {
        public object TestMethod(object value);
    }
}
