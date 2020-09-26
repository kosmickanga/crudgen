using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class DataContextTemplate
    {
        private readonly Model _model;
        private readonly string _namespace;
        private readonly string _context;

        public DataContextTemplate(Model model, string @namespace, string context)
        {
            _model = model;
            this._namespace = @namespace;
            _context = context;
        }
    }
}
