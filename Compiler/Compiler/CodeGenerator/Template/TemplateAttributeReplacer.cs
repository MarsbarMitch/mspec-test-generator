using System;
using System.Collections.Generic;
using System.Linq;
using T = Antlr4.StringTemplate;

namespace Compiler.CodeGenerator.Template
{
    class TemplateAttributeReplacer
    {
        private readonly T.Template template;
        public TemplateAttributeReplacer(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(String.Format(Literals.ArgumentNullMsg, "template"));
            }
            this.template = new T.Template(template);
        }

        public void ReplaceAttribute(string attributeName, string attributeValue)
        {
            template.Add(attributeName, attributeValue);
        }

        public string RenderTemplate()
        {
            return template.Render();
        }
    }
}
