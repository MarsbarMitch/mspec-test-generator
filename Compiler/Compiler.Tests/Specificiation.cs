using System;
using Machine.Specifications;

namespace Compiler.Tests
{
    public abstract class Specification
    {
        protected static Exception Exception { get; private set; }

        Establish context = () =>
        {
            Exception = null;
        };

        protected static Because CatchException(Action action)
        {
            return () => { Exception = Catch.Exception(action); };
        }
    }
}