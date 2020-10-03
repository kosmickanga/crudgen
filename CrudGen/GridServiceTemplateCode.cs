using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class GridServiceTemplate
    {
        private readonly string _namespace;
        private readonly string _serviceName;
        private readonly string _context;
        private readonly string _type;
        private readonly string _query;
        private readonly string _gridName;
        private readonly List<Filter> _filters;

        public GridServiceTemplate(string @namespace, string serviceName, string context, string type, string query, string gridName, List<Filter> filters)
        {
            _namespace = @namespace;
            _serviceName = serviceName;
            _context = context;
            _type = type;
            _query = query;
            _gridName = gridName;
            _filters = filters;
        }
    }
}
