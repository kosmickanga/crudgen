using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CrudGen
{
    public class Model
    {
        public List<Class> Classes { get; set; }
        public List<View> Views { get; set; }
    }

    public class View
    {
        public string Name { get; set; }
        public List<Filter> Filters { get; set; }
        public string ClassName { get; set; }
        // show all columns for now.
    }

    public class Filter
    {
        /// <summary>
        ///  Description in dropdown
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// value in select dropdown
        /// </summary>
        [XmlAttribute]
        public string Value { get; set; }
        /// <summary>
        /// C# code to insert
        /// </summary>
        [XmlAttribute]
        public string Query { get; set; }
        /// <summary>
        ///  true if should be available as submenu of View.
        /// </summary>
        [XmlAttribute]
        public bool ShowInNavBar { get; set; }
    }

    public class Class
    {
        [XmlAttribute]
        public string Name { get; set; }
        public List<Field> Fields { get; set; }

        public string Display { get; set; }

        public Field Key => Fields.First(x => x.Key);
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
        public int MaxLength { get; set; }

        [XmlAttribute]
        public string DbType { get; set; }

        [XmlAttribute]
        public bool Key { get; set; }

        [XmlAttribute]
        public string References { get; set; }

        public bool IsReference => !string.IsNullOrEmpty(References);

        public string ReferenceName => Name + "Id";

        [XmlAttribute]
        public bool Unique { get; set; }
    }
}
