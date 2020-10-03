using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                            new Field {DataType="int", Name="Country", Required = true, References="Country"},
                            new Field {DataType="bool", Name="Done", Required = true},
                            new Field {DataType="DateTime", Name="DueAt", Required = true},
                        },
                     },
                    new Class
                    {
                        Name = "Country",
                        Fields = new List<Field>
                        {
                            new Field {DataType="int", Name="Id", Required=true, Key=true},
                            new Field {DataType="string", Name="Name", Required=true, DbType="nvarchar(255)", MaxLength=255, Unique=true}
                        },
                        Display = "Name"
                    }
                },
                Views = new List<View>
                {
                    new View
                    {
                        ClassName = "Todo",
                        Name = "Todo",
                        Filters = new List<Filter>
                        {
                            new Filter
                            {
                                Name = "All",
                                Value = "all"
                            },
                            new Filter
                            {
                                Name = "Done",
                                Value = "done",
                                Query = "x => x.Done"
                            }
                        }
                    },
                    new View
                    {
                        ClassName = "Country",
                        Name = "Country",
                    }
                }
            };


            using var file = File.Create(@"c:\temp\output.xml");
            sr.Serialize(file, model);
            // https://docs.microsoft.com/en-us/visualstudio/modeling/run-time-text-generation-with-t4-text-templates?view=vs-2019;

            var ns = "TestW";
            var rootFolder = @"C:\users\bahor\source\repos\TestW";
            var appContext = "CrudDbContext";

            var template = new ClassModelTemplate(model, ns);
            var content = template.TransformText();
            var dataFolder = Path.Combine(rootFolder, "Data");
            var modelDest = Path.Combine(dataFolder, "Model.cs");
            var servicesFolder = Path.Combine(rootFolder, "Services");

            Directory.CreateDirectory(dataFolder);
            Directory.CreateDirectory(servicesFolder);

            // Generate class model
            File.WriteAllText(modelDest, content);
            var dcTemplate = new DataContextTemplate(model, ns, appContext);
            var dcContent = dcTemplate.TransformText();
            var dcDest = Path.Combine(dataFolder, "CrudDbContext.cs");
            File.WriteAllText(dcDest, dcContent);
            var servicesToRegister = new List<string>();
            var navLinks = new List<NavLink>();

            // Generate grid services
            foreach (var view in model.Views)
            {
                var m = model.Classes.First(x => x.Name == view.ClassName);

                var references = m.Fields.Where(x => x.IsReference);
                var includes = string.Join("", references.Select(r => $".Include(x => x.{r.Name})"));

                var query = $"{m.Name}{includes}"; //context.Invoice.Include(x => x.InvoiceLines).Include(x => x.Customer).ToList();
                var className = m.Name + "GridService";
                var gridTemplate = new GridServiceTemplate(ns, className, appContext, m.Name, query, m.Name + "Grid", view.Filters);
                var dest = Path.Combine(servicesFolder, className + ".cs");
                var gridServiceContent = gridTemplate.TransformText();
                File.WriteAllText(dest, gridServiceContent);
                servicesToRegister.Add(className);
            }

            // Generate views
            foreach (var view in model.Views)
            {
                var m = model.Classes.First(x => x.Name == view.ClassName);

                var pageName = m.Name.ToLower();
                var filterName = m.Name + "Filter";
                var gridViewTemplate = new GridViewTemplate($"/{pageName}", ns, m.Name, m.Name + "GridService", m.Name + "CrudService", m, view.Filters, filterName);

                navLinks.Add(new NavLink { Url = pageName, Text = view.Name });

                // add View to avoid class name clash with model in XXX.Data
                var viewClass = m.Name + "View";
                var dest = Path.Combine(rootFolder, "Pages", viewClass + ".razor");
                var gridViewContent = gridViewTemplate.TransformText();
                File.WriteAllText(dest, gridViewContent);
                var codeBehindPath = Path.Combine(rootFolder, "Pages", viewClass + ".razor.cs");
                if (File.Exists(codeBehindPath))
                {
                    Console.WriteLine("Not creating file {0} - already exists", codeBehindPath);
                } else
                {
                    var codeBehindTemplate = new GridViewCodeBehindTemplate(ns, viewClass, m.Name);
                    File.WriteAllText(codeBehindPath, codeBehindTemplate.TransformText());
                }

                var filterPath = Path.Combine(rootFolder, "Shared", filterName + ".razor");
                if (view.Filters != null && view.Filters.Any())
                {
                    // add a selector at the top
                    var filterTemplate = new FilterTemplate(view.Filters);
                    File.WriteAllText(filterPath, filterTemplate.TransformText());
                } else
                {
                    if (File.Exists(filterPath))
                    {
                        Console.WriteLine("Please delete unrequired file {0}", filterPath);
                    }
                }
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
