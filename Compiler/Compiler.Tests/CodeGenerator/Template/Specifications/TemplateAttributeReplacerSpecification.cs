using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Compiler.CodeGenerator.Template;

namespace Compiler.Tests.CodeGenerator.Template.Specifications
{
    abstract class TemplateAttributeReplacerSpecification : Specification
    {
        protected static TemplateAttributeReplacer Sut;
        protected static string TemplateBody = "{0} are by far the greatest team the world has ever seen";
        protected static string Template;
        protected static string SingleAttribute = "FOOTBALL_CLUB";
        Establish context = () => 
        {
            
        };

        protected static void BuildTemplateWithSingleAttribute()
        {
            //Template = String.Format("<{0}>"TemplateBody, SingleAttribute);
            Template = String.Format(TemplateBody, String.Format("<{0}>", SingleAttribute));
        }

        protected static void CreateSut(string template)
        {
            Sut = new TemplateAttributeReplacer(template);
        }
    }
}
