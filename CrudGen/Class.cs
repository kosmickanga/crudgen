using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CrudGen
{
    public class Model
    {
        public List<Class> Classes { get; set; }
    }

    public class Class
    {
        [XmlAttribute]
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
    }

    public class Field
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public bool Required { get; set; }
        [XmlAttribute]
        public string DataType { get; set; }

        [XmlAttribute]
        public string DefaultValue { get; set; }

        [XmlAttribute]
        public string MaxLength { get; set; }

        [XmlAttribute]
        public string DbType { get; set; }

        [XmlAttribute]
        public bool Key { get; set; }
    }
}
