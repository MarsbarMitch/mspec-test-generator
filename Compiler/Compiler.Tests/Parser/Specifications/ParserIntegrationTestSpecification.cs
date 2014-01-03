using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using C = Compiler.ZestParser;
using G = Compiler.ZestGrammar;
using System.Reflection;
using System.IO;

namespace Compiler.Tests.Parser.Specifications
{
    class ParserIntegrationTestSpecification : Specification
    {
        protected static C.ZestParser Sut;
        protected static string ValidSourceCodeResource = "Compiler.Tests.Parser.Resources.Valid.zst";
        protected static string InvalidSourceCodeResource = "Compiler.Tests.Parser.Resources.InvalidWhen.zst";

        Establish context = () => { };

        protected static void CreateSut(SourceCode sourceCode)
        {
            Sut = new C.ZestParser(new G.ZestGrammar(), sourceCode);
        }

        protected static string ReadInSourceCode(string resource)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream input = assembly.GetManifestResourceStream(resource))
            {
                using (StreamReader reader = new StreamReader(input))
                {
                    return reader.ReadToEnd();
                }
            }
        }

    }
}
