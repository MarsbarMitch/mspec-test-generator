using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Compiler.CodeGenerator.Template;
using Compiler.Tests.CodeGenerator.Template.Specifications;

namespace Compiler.Tests.CodeGenerator.Template
{
    [Subject(typeof(TemplateReader))]
    class when_calling_LoadTemplate_with_null_resource : TemplateReaderSpecification
    {
        private static string result;
        Because of = CatchException(() => result = TemplateReader.ReadTemplate(null));
        It should_throw_an_exception = () => Exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof(TemplateReader))]
    class when_calling_LoadTemplate_with_non_existing_resource: TemplateReaderSpecification
    {
        private static string result;
        Because of = CatchException(() => result = TemplateReader.ReadTemplate(ErrorResource));
        It should_throw_an_exception = () => Exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof(TemplateReader))]
    class when_calling_LoadTemplate_with_existing_resource : TemplateReaderSpecification
    {
        private static string result;
        Because of = () => result = TemplateReader.ReadTemplate(ValidResource);
        It should_return_the_template = () => result.ShouldContain("Template with valid template tags");
    }
}
