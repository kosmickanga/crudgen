using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class CrudServiceRegistrationTemplate
    {
        private string _namespace;
        private List<string> _servicesToRegister;

        public CrudServiceRegistrationTemplate(string @namespace, List<string> servicesToRegister)
        {
            _namespace = @namespace;
            _servicesToRegister = servicesToRegister;
        }
    }
}
