using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class ClassModelTemplate
    {
        private readonly Model _model;
        private readonly string _namespace;

        public ClassModelTemplate(Model model, string @namespace)
        {
            _model = model;
            _namespace = @namespace;
        }

        public string GetDisplay(Class @class)
        {
            // TODO - improve what can be displayed.
            return $"public string Display => {@class.Display};";
        }
    }
}
