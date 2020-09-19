using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class DataContextTemplate
    {
        private readonly Model _model;
        private readonly string _namespace;

        public DataContextTemplate(Model model, string @namespace)
        {
            _model = model;
            this._namespace = @namespace;
        }
    }
}
