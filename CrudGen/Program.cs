using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace CrudGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CrudGen generating crud");

            // Templates use Invariant culture so embedded datestamps are mm/dd/yyyy.
            var inflector = new Inflector.Inflector(Thread.CurrentThread.CurrentUICulture);

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

            var ns = "TestY";
            var rootFolder = @"C:\users\bahor\source\repos\TestY";
            var appContext = "CrudDbContext";

            var template = new ClassModelTemplate(model, ns);
            var content = template.TransformText();
            var dataFolder = Path.Combine(rootFolder, "Data");
            var modelDest = Path.Combine(dataFolder, "Model.cs");
            var servicesFolder = Path.Combine(rootFolder, "Services");

            Directory.CreateDirectory(dataFolder);
            Directory.CreateDirectory(servicesFolder);

            File.WriteAllText(modelDest, content);
            var dcTemplate = new DataContextTemplate(model, ns, appContext);
            var dcContent = dcTemplate.TransformText();
            var dcDest = Path.Combine(dataFolder, "CrudDbContext.cs");
            File.WriteAllText(dcDest, dcContent);
            var servicesToRegister = new List<string>();
            var navLinks = new List<NavLink>();

            foreach (var m in model.Classes)
            {
                var references = m.Fields.Where(x => x.IsReference);
                var includes = string.Join("", references.Select(r => $".Include(x => x.{r.Name})"));

                var query = $"{m.Name}{includes}.ToList()"; //context.Invoice.Include(x => x.InvoiceLines).Include(x => x.Customer).ToList();
                var className = m.Name + "GridService";
                var gridTemplate = new GridServiceTemplate(ns, className, appContext, m.Name, query, m.Name + "Grid");
                var dest = Path.Combine(servicesFolder, className + ".cs");
                var gridServiceContent = gridTemplate.TransformText();
                File.WriteAllText(dest, gridServiceContent);
                servicesToRegister.Add(className);
            }
            foreach (var m in model.Classes)
            {
                var pageName = m.Name.ToLower();
                var gridViewTemplate = new GridViewTemplate($"/{pageName}", ns, m.Name, m.Name + "GridService", m.Name + "CrudService", m);
                navLinks.Add(new NavLink { Url = pageName, Text = inflector.Pluralize(m.Name) });

                // add View to avoid class name clash with model in XXX.Data
                var dest = Path.Combine(rootFolder, "Pages", m.Name + "View.razor");
                var gridViewContent = gridViewTemplate.TransformText();
                File.WriteAllText(dest, gridViewContent);
            }
            foreach (var m in model.Classes)
            {
                var className = m.Name + "CrudService";
                var crudServiceTemplate = new CrudServiceTemplate(ns, className, appContext, m.Name);
                var crudServiceContent = crudServiceTemplate.TransformText();
                var dest = Path.Combine(servicesFolder, className + ".cs");
                File.WriteAllText(dest, crudServiceContent);
                servicesToRegister.Add(className);
            }
            foreach (var reference in model.Classes.SelectMany(x => x.Fields).Where(x => x.IsReference).Select(x => x.References).Distinct())
            {
                var @class = model.Classes.First(x => x.Name == reference);
                var className = @class.Name + "LookupService";
                var lookupServiceTemplate = new LookupServiceTemplate(appContext, ns, className, @class.Key.Name, @class);
                var lsContent = lookupServiceTemplate.TransformText();
                var dest = Path.Combine(servicesFolder, className + ".cs");
                File.WriteAllText(dest, lsContent);
                servicesToRegister.Add(className);
            }
            
            // Service registration with DI container
            var regTemplate = new CrudServiceRegistrationTemplate(ns, servicesToRegister);
            var regContent = regTemplate.TransformText();
            var regCsPath = Path.Combine(rootFolder, "CrudServiceRegistration.cs");
            File.WriteAllText(regCsPath, regContent);

            var navMenuTemplate = new NavMenuTemplate(navLinks);
            var navMenuPath = Path.Combine(rootFolder, "Shared", "CrudNavMenu.razor");
            File.WriteAllText(navMenuPath, navMenuTemplate.TransformText());

        }
    }
}
