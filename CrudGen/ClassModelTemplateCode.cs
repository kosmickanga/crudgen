using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class ClassModelTemplate
    {
        private readonly Model _model;

        public ClassModelTemplate(Model model)
        {
            _model = model;
        }
    }
}
