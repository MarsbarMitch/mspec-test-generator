using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Tests.CodeGenerator.Template.Specifications;
using Machine.Specifications;
using Compiler.CodeGenerator.Template;

namespace Compiler.Tests.CodeGenerator.Template
{
    [Subject(typeof(TemplateAttributeReplacer))]
    class when_constructing_with_null_template : TemplateAttributeReplacerSpecification
    {
        Because of = CatchException(() => CreateSut(null));
        It should_throw_an_exception = () => Exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof(TemplateAttributeReplacer))]
    class when_constructing_with_template : TemplateAttributeReplacerSpecification
    {
        Establish context = () => BuildTemplateWithSingleAttribute();
        Because of = CatchException(() => CreateSut(Template));
        It should_create_a_sut = () => Sut.ShouldNotBeNull();
    }

    [Subject(typeof(TemplateAttributeReplacer))]
    class when_using_TemplateAttributeReplacer_to_replace_single_attribute : TemplateAttributeReplacerSpecification
    {
        private static string AttributeValue =  "Arsenal";
        Establish context = () =>
        {
            BuildTemplateWithSingleAttribute();
            CreateSut(Template);
        };
        Because of = () => Sut.ReplaceAttribute(SingleAttribute, AttributeValue);
        It should_replace_attribute = () => Sut.RenderTemplate().ShouldEqual(String.Format(TemplateBody, AttributeValue));

    }
}
