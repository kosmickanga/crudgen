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

            var ns = "TestApp";
            var nsd = "TestApp.Data";
            var rootFolder = @"C:\users\bahor\source\TestApp";

            var template = new ClassModelTemplate(model, nsd);
            var content = template.TransformText();
            var dataFolder = Path.Combine(rootFolder, "Data");
            var modelDest = Path.Combine(dataFolder, "Model.cs");
            Directory.CreateDirectory(dataFolder);

            File.WriteAllText(modelDest, content);
            var dcTemplate = new DataContextTemplate(model, ns);
            var dcContent = dcTemplate.TransformText();
            var dcDest = Path.Combine(rootFolder, "AppDbContext.cs");

            foreach (var m in model.Classes)
            {
                var query = $"{m.Name}.ToList()"; //context.Invoice.Include(x => x.InvoiceLines).Include(x => x.Customer).ToList();
                var gridTemplate = new GridServiceTemplate(ns, m.Name + "GridService", "AppDbContext", m.Name, query, m.Name + "Grid");
                var dest = Path.Combine(rootFolder, m.Name + "GridService.cs");
                var gridServiceContent = gridTemplate.TransformText();
                File.WriteAllText(dest, gridServiceContent);
            }
            foreach (var m in model.Classes)
            {
                var gridViewTemplate = new GridViewTemplate($"/{m.Name.ToLower()}", ns, m.Name, m.Name + "GridService", m.Name + "CrudService", m);
                
                // add View to avoid class name clash with model in XXX.Data
                var dest = Path.Combine(rootFolder, "Pages", m.Name + "View.razor");
                var gridViewContent = gridViewTemplate.TransformText();
                File.WriteAllText(dest, gridViewContent);
            }
            foreach (var m in model.Classes)
            {
                var crudServiceTemplate = new CrudServiceTemplate(nsd, m.Name + "CrudService", "AppDbContext", m.Name);
                var crudServiceContent = crudServiceTemplate.TransformText();
                var dest = Path.Combine(dataFolder, m.Name + "CrudService.cs");
                File.WriteAllText(dest, crudServiceContent);
            }
        }
    }
}
