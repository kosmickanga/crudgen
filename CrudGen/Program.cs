using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace CrudGen
{
    class Program
    {
        static int Main(string[] args)
        {
            var newCommand = new Command("new", "Creates a new project")
            {
                new Option<string>(new string[] {"-o", "--output" }, "Location of folder"),
                new Option<string>(new string[] {"-n", "--name"}, "Name of project to create as subfolder"){IsRequired = true },
                new Option<string>(new string[] {"-x", "--xml"}, "XML Schema file" ) {IsRequired = true },
                new Option<bool>(new string[] {"-f", "--force"},  () => false, "Force overwrite" ),
            };
            newCommand.Handler = CommandHandler.Create<string, string, string, bool>(DoNew);

            var updateCommand = new Command("update", "Updates an existing project")
            {
                new Option<string>(new string[] {"-o", "--output" }, "Project folder") {IsRequired = true },
                new Option<string>(new string[] {"-n", "--name"}, "Namespace"){IsRequired = true },
                new Option<string>(new string[] {"-x", "--xml"}, "XML Schema file" ) {IsRequired = true },
            };
            updateCommand.Handler = CommandHandler.Create<string, string, string>(DoUpdate);

            var sampleGenCommand = new Command("samplegen", "Generate sample XML schema (for testing)")
            {
                new Option<string>(new string[] {"-o", "--output" }, "Output file (eg schema.xml)") {IsRequired = true },
            };
            sampleGenCommand.Handler = CommandHandler.Create<string>(GenSample);

            var rootCommand = new RootCommand
            {
                new Option<string>("--dest-folder"),
                newCommand,
                updateCommand,
                new Command("migrations"),
                new Command("database"),
                sampleGenCommand
            };
            rootCommand.Description = "CrudGen generates a Blazor Project from a single schema file";
            rootCommand.Handler = CommandHandler.Create<string>((destFolder) =>
            {
                Console.WriteLine($"dest folder is: {destFolder}");
            });
            return rootCommand.InvokeAsync(args).Result;
        }

        private static bool RunProc(System.Diagnostics.Process process)
        {
            using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
            {
                process.OutputDataReceived += (sender, e) => {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        Console.Out.WriteLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        Console.Error.WriteLine(e.Data);
                    }
                };

                // process.Start(); // already running

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                var timeoutMs = 90 * 1000;

                if (process.WaitForExit(timeoutMs) &&
                    outputWaitHandle.WaitOne(timeoutMs) &&
                    errorWaitHandle.WaitOne(timeoutMs))
                {
                    // Process completed. Check process.ExitCode here.
                    return true;
                }
                else
                {
                    // Timed out.
                    return false;
                }
            }
        }
        /// <summary>
        ///  Creates a new project
        /// </summary>
        /// <param name="output"></param>
        /// <param name="name"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        static int DoNew(string output, string name, string xml, bool force)
        {
            if (string.IsNullOrEmpty(output)) 
                output = Directory.GetCurrentDirectory();
            var dest = Path.Combine(output, name);

            var startInfo = new ProcessStartInfo()
            {
                FileName = "dotnet.exe",
                ArgumentList =
                {
                    "new", "crudgenblazorserver", "-au", "Individual", "-uld", "-n", name, "-o", dest
                },
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            if (force)
            {
                startInfo.ArgumentList.Add("--force");
            }

            using (var proc = System.Diagnostics.Process.Start(startInfo))
            {
                var ranOk = RunProc(proc);
                if (!ranOk || proc.ExitCode != 0)
                {
                    Console.Error.WriteLine("Failed creating new project");
                    return 5;
                }
            }

            if (DoUpdate(dest, name, xml) != 0)
            {
                Console.Error.WriteLine("Failed processing XML schema");
                return 6;
            }

            var siMigrations = new ProcessStartInfo()
            {
                FileName = "dotnet-ef.exe",
                ArgumentList =
                {
                    "migrations", "add", "-o", @"Data\CrudMigrations", "-c", "CrudDbContext", "InitialCreate"
                },
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = dest
            };
            using (var proc = System.Diagnostics.Process.Start(siMigrations))
            {
                var ranOk = RunProc(proc);
                if (!ranOk || proc.ExitCode != 0)
                {
                    Console.Error.WriteLine("Failed generating migration");
                    return 7;
                }
            }


            // "dotnet new crudgenblazorserver - au Individual - n TestZ - o TestZ - uld";
            // then
            // "dotnet-ef migrations add -o Data\CrudMigrations -c CrudDbContext InitialCreate"
            // then

            // build
            // "dotnet-ef database update -c CrudDbContext"
            // "dotnet-ef database update -c ApplicationDbContext"

            return 0;
        }
        /// <summary>
        ///  Updates a project
        /// </summary>
        /// <param name="output">Project root folder</param>
        /// <param name="name">Name (=== namespace)</param>
        /// <param name="xml">XML file what is wanted.</param>
        /// <returns></returns>
        static int DoUpdate(string output, string name, string xml)
        {
            Console.WriteLine($"Updating {output} {name} {xml}");
            var sr = new XmlSerializer(typeof(Model));
            using var stream = File.OpenRead(xml);
            var model = (Model) sr.Deserialize(stream);

            // Templates use Invariant culture so embedded datestamps are mm/dd/yyyy.
            var inflector = new Inflector.Inflector(Thread.CurrentThread.CurrentUICulture);

            // https://docs.microsoft.com/en-us/visualstudio/modeling/run-time-text-generation-with-t4-text-templates?view=vs-2019;

            var appContext = "CrudDbContext";

            var template = new ClassModelTemplate(model, name);
            var content = template.TransformText();
            var dataFolder = Path.Combine(output, "Data");
            var modelDest = Path.Combine(dataFolder, "Model.cs");
            var servicesFolder = Path.Combine(output, "Services");

            Directory.CreateDirectory(dataFolder);
            Directory.CreateDirectory(servicesFolder);

            // Generate class model
            File.WriteAllText(modelDest, content);
            var dcTemplate = new DataContextTemplate(model, name, appContext);
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
                var gridTemplate = new GridServiceTemplate(name, className, appContext, m.Name, query, m.Name + "Grid", view.Filters);
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
                var gridViewTemplate = new GridViewTemplate($"/{pageName}", name, m.Name, m.Name + "GridService", m.Name + "CrudService", m, view.Filters, filterName);

                navLinks.Add(new NavLink { Url = pageName, Text = view.Name });

                // add View to avoid class name clash with model in XXX.Data
                var viewClass = m.Name + "View";
                var dest = Path.Combine(output, "Pages", viewClass + ".razor");
                var gridViewContent = gridViewTemplate.TransformText();
                File.WriteAllText(dest, gridViewContent);
                var codeBehindPath = Path.Combine(output, "Pages", viewClass + ".razor.cs");
                if (File.Exists(codeBehindPath))
                {
                    Console.WriteLine("Not creating file {0} - already exists", codeBehindPath);
                }
                else
                {
                    var codeBehindTemplate = new GridViewCodeBehindTemplate(name, viewClass, m.Name);
                    File.WriteAllText(codeBehindPath, codeBehindTemplate.TransformText());
                }

                var filterPath = Path.Combine(output, "Shared", filterName + ".razor");
                if (view.Filters != null && view.Filters.Any())
                {
                    // add a selector at the top
                    var filterTemplate = new FilterTemplate(view.Filters);
                    File.WriteAllText(filterPath, filterTemplate.TransformText());
                }
                else
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
                var crudServiceTemplate = new CrudServiceTemplate(name, className, appContext, m.Name);
                var crudServiceContent = crudServiceTemplate.TransformText();
                var dest = Path.Combine(servicesFolder, className + ".cs");
                File.WriteAllText(dest, crudServiceContent);
                servicesToRegister.Add(className);
            }
            foreach (var reference in model.Classes.SelectMany(x => x.Fields).Where(x => x.IsReference).Select(x => x.References).Distinct())
            {
                var @class = model.Classes.First(x => x.Name == reference);
                var className = @class.Name + "LookupService";
                var lookupServiceTemplate = new LookupServiceTemplate(appContext, name, className, @class.Key.Name, @class);
                var lsContent = lookupServiceTemplate.TransformText();
                var dest = Path.Combine(servicesFolder, className + ".cs");
                File.WriteAllText(dest, lsContent);
                servicesToRegister.Add(className);
            }

            // Service registration with DI container
            var regTemplate = new CrudServiceRegistrationTemplate(name, servicesToRegister);
            var regContent = regTemplate.TransformText();
            var regCsPath = Path.Combine(output, "CrudServiceRegistration.cs");
            File.WriteAllText(regCsPath, regContent);

            var navMenuTemplate = new NavMenuTemplate(navLinks);
            var navMenuPath = Path.Combine(output, "Shared", "CrudNavMenu.razor");
            File.WriteAllText(navMenuPath, navMenuTemplate.TransformText());



            return 0;
        }
        /// <summary>
        ///  Generates a sample model (mainly for testing purposes)
        /// </summary>
        /// <param name="output">path to output file</param>
        /// <returns></returns>
        static int GenSample(string output)
        {
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
                            new Field {DataType="DateTime", Name="DueAt", Required = true, Format="{0:yyyy-MM-dd}"},
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


            using var file = File.Create(output);
            sr.Serialize(file, model);

            return 0;
        }
    }
}
