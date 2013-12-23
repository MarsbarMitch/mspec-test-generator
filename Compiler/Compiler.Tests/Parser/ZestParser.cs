using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using C = Compiler.ZestParser;
using Compiler.Tests.Parser.Specifications;

namespace Compiler.Tests.Parser
{
    [Subject(typeof(C.ZestParser))]
    class when_calling_isValid : ParserIntegrationTestSpecification
    {
        private static bool result;
        Establish context = () => CreateSut();
        Because of = () => result = Sut.isSourceCodeValid(ReadInSourceCode(ValidSourceCodeResource));
        It should_return_true = () => result.ShouldBeTrue();
    }
}
