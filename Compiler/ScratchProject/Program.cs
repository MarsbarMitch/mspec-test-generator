using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using ScratchProject.GridWorldDemo;

namespace ScratchProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceCode = ReadInSourceFile("ScratchProject.SourceFiles.Simple.gw");
            /*GridWorldGrammar grammar = new GridWorldGrammar();
            Compiler.Compiler compiler = new Compiler.Compiler(grammar);
            if (compiler.isValid(sourceCode))
            {
                //compiler.DisplayTree(compiler.GetRoot(sourceCode), 0);
                compiler.CompileCode(sourceCode);
            }
            else
            {
                Console.WriteLine("Compiler error");
            }*/
			AstCompile(sourceCode);
        }

        private static string ReadInSourceFile(string resource)
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
		
		private static void AstCompile(String sourceCode)
		{
			GridWorldGrammar grammar = new GridWorldGrammar();
			VisitorCompiler vc = new VisitorCompiler(grammar);
			var astRoot = vc.GetAstRoot (sourceCode);
			Console.WriteLine(astRoot.ToString());
		}
    }
}
