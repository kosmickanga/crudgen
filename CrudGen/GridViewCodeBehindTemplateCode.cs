using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class GridViewCodeBehindTemplate
    {
        private readonly string _namespace;
        private readonly string _viewClass;
        private readonly string _type;

        public GridViewCodeBehindTemplate(string @namespace, string viewClass, string type)
        {
            _namespace = @namespace;
            _viewClass = viewClass;
            _type = type;
        }
    }
}
