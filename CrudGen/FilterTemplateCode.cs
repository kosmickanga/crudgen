using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class FilterTemplate
    {
        private readonly List<Filter> _filters;

        public FilterTemplate(List<Filter> filters)
        {
            _filters = filters;
        }
    }
}
