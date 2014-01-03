using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Compiler.Command.Interfaces;
using Compiler.Command;
using Irony.Parsing;

namespace Compiler.Tests.Command.Specifications
{
    abstract class CommandFactorySpecification : Specification
    {
        protected static ICommandFactory Sut;
        protected static SourceCode DefaultSourceCode;
        protected static Grammar DefalutGrammer;
        Establish context = () => 
        {
            DefaultSourceCode = new SourceCode("source code");
            DefalutGrammer = new Grammar();
        };

        protected static void CreatesSut()
        {
            Sut = new CommandFactory(DefaultSourceCode, DefalutGrammer);
        }

    }
}
