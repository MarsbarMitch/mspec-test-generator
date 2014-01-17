using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Compiler.CodeGenerator.Template
{
    class TemplateReader
    {
        public static string ReadTemplate(string templateResource)
        {
            using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(templateResource)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
