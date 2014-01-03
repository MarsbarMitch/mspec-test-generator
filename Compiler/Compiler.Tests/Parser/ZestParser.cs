using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using C = Compiler.ZestParser;
using Compiler.Tests.Parser.Specifications;
using Irony.Parsing;
using Compiler.ZestParser;

namespace Compiler.Tests.Parser
{
    [Subject(typeof(C.ZestParser))]
    class when_calling_Parse_with_valid_code : ParserIntegrationTestSpecification
    {
        private static ParsedSourceCode result;
        Establish context = () => CreateSut();
        Because of = () => result = Sut.Parse(ReadInSourceCode(ValidSourceCodeResource));
        It should_have_no_errors = () => result.HasErrors.ShouldBeFalse();
        It should_set_have_parse_tree_root = () => result.ParseTreeRoot.ShouldNotBeNull();
    }
    
    [Subject(typeof(C.ZestParser))]
    class when_calling_parsing_invalid_source_code : ParserIntegrationTestSpecification
    {
        private static ParsedSourceCode result;
        Establish context = () => CreateSut();
        Because of = () => result = Sut.Parse(ReadInSourceCode(InvalidSourceCodeResource));
        It should_have_errors = () => result.HasErrors.ShouldBeTrue();
        It should_have_one_error = () => result.ErrorList.Count.ShouldEqual(1);
    }
}
