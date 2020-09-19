using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class CrudServiceTemplate
    {
        private string _namespace;
        private string _dataContext;
        private string _class;
        private string _serviceName;

        public CrudServiceTemplate(string @namespace, string serviceName, string dataContext, string @class) 
        {
            _namespace = @namespace;
            _dataContext = dataContext;
            _class = @class;
            _serviceName = serviceName;
        }
    }
}
