using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                Classes = new List<Class> 
                {
                    new Class
                    {
                        Name = "Todo",
                        Fields = new List<Field>
                        {
                            new Field {DataType="int", Name="Id", Required = true, Key=true},
                            new Field {DataType="string", Name="Text", Required = true, DbType="nvarchar(max)"},
                            new Field {DataType="int", Name="Country", Required = true, References="Country"}
                        },
                     },
                    new Class
                    {
                        Name = "Country",
                        Fields = new List<Field>
                        {
                            new Field {DataType="int", Name="Id", Required=true, Key=true},
                            new Field {DataType="string", Name="Name", Required=true, DbType="nvarchar(255)"}
                        },
                        Display = "Name"
                    }
                },
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
            File.WriteAllText(dcDest, dcContent);
            var appContext = "AppDbContext";
            foreach (var m in model.Classes)
            {
                var references = m.Fields.Where(x => x.IsReference);
                var includes = string.Join("", references.Select(r => $".Include(x => x.{r.Name})"));

                var query = $"{m.Name}{includes}.ToList()"; //context.Invoice.Include(x => x.InvoiceLines).Include(x => x.Customer).ToList();
                var gridTemplate = new GridServiceTemplate(ns, m.Name + "GridService", appContext, m.Name, query, m.Name + "Grid");
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
                var crudServiceTemplate = new CrudServiceTemplate(nsd, m.Name + "CrudService", appContext, m.Name);
                var crudServiceContent = crudServiceTemplate.TransformText();
                var dest = Path.Combine(dataFolder, m.Name + "CrudService.cs");
                File.WriteAllText(dest, crudServiceContent);
            }
            foreach (var reference in model.Classes.SelectMany(x => x.Fields).Where(x => x.IsReference).Select(x => x.References).Distinct())
            {
                var @class = model.Classes.First(x => x.Name == reference);
                var lookupServiceTemplate = new LookupServiceTemplate(appContext, ns, @class.Name + "LookupService", @class.Key.Name, @class);
                var lsContent = lookupServiceTemplate.TransformText();
                var dest = Path.Combine(rootFolder, @class.Name + "LookupService.cs");
                File.WriteAllText(dest, lsContent);
            }
        }
    }
}
