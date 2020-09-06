using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public class Class
    {
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
    }

    public class Field
    {
        public string Name { get; set; }
        public bool Required { get; set; }
        public string DataType { get; set; }

        public string DefaultValue { get; set; }
    }
}
