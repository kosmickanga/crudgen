using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class LookupServiceTemplate
    {
        private string _namespace;
        private Class _class;
        private string _serviceName;
        private string _appContext;
        private string _id;

        public LookupServiceTemplate(string appContext, string @namespace, string serviceName, string id, Class @class)
        {
            _namespace = @namespace;
            _class = @class;
            _serviceName = serviceName;
            _appContext = appContext;
            _id = id;
        }
    }
}
