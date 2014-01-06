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
        protected static ImmediateRepresentation DefaultImmediateRep;
        protected static Grammar DefalutGrammer;
        Establish context = () => 
        {
            DefaultImmediateRep = new ImmediateRepresentation("source code");
            DefalutGrammer = new Grammar();
        };

        protected static void CreatesSut()
        {
            Sut = new CommandFactory(DefaultImmediateRep, DefalutGrammer);
        }

    }
}
