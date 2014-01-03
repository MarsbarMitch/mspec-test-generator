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
        private static SourceCode sourceCode;
        Establish context = () =>
        {
            sourceCode = new SourceCode(ReadInSourceCode(ValidSourceCodeResource));
            CreateSut(sourceCode);
        };
        Because of = () => Sut.Execute();
        It should_have_no_errors = () => sourceCode.HasErrors.ShouldBeFalse();
        It should_set_have_parse_tree_root = () => sourceCode.ParseTreeRoot.ShouldNotBeNull();
    }
    
    [Subject(typeof(C.ZestParser))]
    class when_calling_parsing_invalid_source_code : ParserIntegrationTestSpecification
    {
        private static SourceCode sourceCode;
        Establish context = () =>
        {
            sourceCode = new SourceCode(ReadInSourceCode(InvalidSourceCodeResource));
            CreateSut(sourceCode);
        };
        Because of = () => Sut.Execute();
        It should_have_errors = () => sourceCode.HasErrors.ShouldBeTrue();
        It should_have_one_error = () => sourceCode.ErrorList.Count.ShouldEqual(1);
    }
}
