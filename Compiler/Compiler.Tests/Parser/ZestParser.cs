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
        private static ImmediateRepresentation immediateRep;
        Establish context = () =>
        {
            immediateRep = new ImmediateRepresentation(ReadInSourceCode(ValidSourceCodeResource));
            CreateSut(immediateRep);
        };
        Because of = () => Sut.Execute();
        It should_have_no_errors = () => immediateRep.HasErrors.ShouldBeFalse();
        It should_set_have_parse_tree_root = () => immediateRep.ParseTreeRoot.ShouldNotBeNull();
    }
    
    [Subject(typeof(C.ZestParser))]
    class when_calling_parsing_invalid_source_code : ParserIntegrationTestSpecification
    {
        private static ImmediateRepresentation immediateRep;
        Establish context = () =>
        {
            immediateRep = new ImmediateRepresentation(ReadInSourceCode(InvalidSourceCodeResource));
            CreateSut(immediateRep);
        };
        Because of = () => Sut.Execute();
        It should_have_errors = () => immediateRep.HasErrors.ShouldBeTrue();
        It should_have_one_error = () => immediateRep.ErrorList.Count.ShouldEqual(1);
    }
}
