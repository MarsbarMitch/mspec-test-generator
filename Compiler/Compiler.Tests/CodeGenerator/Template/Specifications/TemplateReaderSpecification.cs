using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;


namespace Compiler.Tests.CodeGenerator.Template.Specifications
{
    abstract class TemplateReaderSpecification : Specification
    {
        protected static string ErrorResource = "arsenal.fc";
        protected static string ValidResource = "Compiler.CodeGenerator.Template.CsharpTemplates.TemplateForTesting.ztt";
        Establish context = () => { };
    }
}
