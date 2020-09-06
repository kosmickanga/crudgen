using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CrudGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var sr = new XmlSerializer(typeof(Model));
            var model = new Model
            {
                Classes = new List<Class> {
             new Class
            {
                Name = "Todo",
                Fields = new List<Field>
                {
                    new Field {DataType="int", Name="Id", Required = true, Key=true},
                    new Field {DataType="string", Name="Text", Required = true, DbType="nvarchar(max)"}
                }
            }
                }
            };
            using var file = File.Create(@"c:\temp\output.xml");
            sr.Serialize(file, model);
            // https://docs.microsoft.com/en-us/visualstudio/modeling/run-time-text-generation-with-t4-text-templates?view=vs-2019;

            var template = new ClassModelTemplate(model);
            var content = template.TransformText();
            File.WriteAllText(@"c:\temp\output.cs", content);

        }
    }
}
