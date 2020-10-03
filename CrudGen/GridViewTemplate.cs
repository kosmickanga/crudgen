﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace CrudGen
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.Reflection;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class GridViewTemplate : GridViewTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("@page \"");
            
            #line 8 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_page));
            
            #line default
            #line hidden
            this.Write("\"\r\n");
            
            #line 9 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 if (_filters.Any()) { 
            
            #line default
            #line hidden
            this.Write("@page \"");
            
            #line 10 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_page));
            
            #line default
            #line hidden
            this.Write("/{state}\"\r\n");
            
            #line 11 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("@attribute [Authorize]\r\n\r\n@using GridBlazor\r\n@using GridBlazor.Pages\r\n@using Grid" +
                    "Shared\r\n@using GridShared.Utility\r\n@using Microsoft.Extensions.Primitives\r\n@usin" +
                    "g CrudGen.Runtime\r\n@using ");
            
            #line 20 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".Data\r\n@using ");
            
            #line 21 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".Services\r\n\r\n@*\r\nAutogenerated file, do not edit. \r\nCreated ");
            
            #line 25 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DateTime.Now));
            
            #line default
            #line hidden
            this.Write("\r\nCrudGen V");
            
            #line 26 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version));
            
            #line default
            #line hidden
            this.Write("\r\n*@\r\n\r\n\r\n@inject ");
            
            #line 30 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_gridServiceName));
            
            #line default
            #line hidden
            this.Write(" GridService\r\n@inject ");
            
            #line 31 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_crudServiceName));
            
            #line default
            #line hidden
            this.Write(" CrudService\r\n\r\n");
            
            #line 33 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 foreach (var r in GetRefFieldNames()) {

            
            #line default
            #line hidden
            this.Write("@inject ");
            
            #line 35 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(LookupServiceName(r)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 35 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(LookupServiceInst(r)));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 36 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"

}

            
            #line default
            #line hidden
            this.Write("\r\n@if (_task.IsCompleted)\r\n{\r\n");
            
            #line 42 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 if (_filters.Any()) { 
            
            #line default
            #line hidden
            this.Write("    <");
            
            #line 43 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_filterClass));
            
            #line default
            #line hidden
            this.Write(" State=@State OnStateChanged=@FilterStateChanged /> \r\n");
            
            #line 44 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("   <GridComponent T=\"");
            
            #line 45 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write("\" Grid=\"@_grid\" @ref=_gridComponent></GridComponent>\r\n}\r\nelse\r\n{\r\n    <p><em>Load" +
                    "ing...</em></p>\r\n}\r\n\r\n@code {\r\n    private CGrid<");
            
            #line 53 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write("> _grid;\r\n    private Task _task;\r\n    private GridComponent<");
            
            #line 55 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write("> _gridComponent;\r\n    private bool _initGridComponent;\r\n");
            
            #line 57 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 if (_filters.Any()) { 
            
            #line default
            #line hidden
            this.Write(@"    [Parameter]
    public string State {get; set;}

    private async Task FilterStateChanged(string newValue) 
    {
        //_state = args.Value.ToString();
        State = newValue;
        // Debug.Print(""Parent: We have a change => {0}"", State);
        _grid.AddQueryParameter(""state"", State);
        await _gridComponent?.UpdateGrid();
    }

    protected override void OnInitialized()
    {
        State = State ?? ""all""; // returns all by default due to switch fall through.
    }
");
            
            #line 74 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"
    protected override void OnAfterRender(bool firstRender) 
    {
        if (!_initGridComponent && _gridComponent != null) 
        {
            _gridComponent.BeforeInsert += BeforeInsert;
            _gridComponent.BeforeUpdate += BeforeUpdate;
            _initGridComponent = true;
        }
    }
#region Validation
    private bool ValidateItem(ValidationArgs args, ");
            
            #line 86 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write(" c) \r\n    {\r\n        // generated code will be inserted here.\r\n        return tru" +
                    "e;\r\n    }\r\n\r\n    private async Task<bool> BeforeUpdate(GridUpdateComponent<");
            
            #line 92 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write("> gridComponent, ");
            
            #line 92 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write(@" c)
    {
        var args = new ValidationArgs(false, gridComponent.ColumnErrors);
        if (!ValidateItem(args, c)) 
        {
            gridComponent.Error = args.Error;
            return false;
        }
        return await DoValidateAsync(args, c);

    }
    private async Task<bool> BeforeInsert(GridCreateComponent<");
            
            #line 103 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write("> gridComponent, ");
            
            #line 103 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write(@" c) 
    {
        var args = new ValidationArgs(false, gridComponent.ColumnErrors);
        if (!ValidateItem(args, c)) 
        {
            gridComponent.Error = args.Error;
            return false;
        }

        return await Task.FromResult(true);
    }

#endregion


    protected override async Task OnParametersSetAsync()
    {
        Action<IGridColumnCollection<");
            
            #line 120 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write(">> columns = c =>\r\n        {\r\n");
            
            #line 122 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 foreach (var f in _class.Fields) 
{ 
            
            #line default
            #line hidden
            this.Write("            ");
            
            #line 124 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GenerateColumn(f)));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 125 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("        };\r\n\r\n        var query = new QueryDictionary<StringValues>();\r\n        q" +
                    "uery.Add(\"grid-page\", \"2\");\r\n\r\n        var client = new GridClient<");
            
            #line 131 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write(">(q => GridService.GetGridRowsAsync(columns, q), query, false, \"");
            
            #line 131 "C:\Users\bahor\source\repos\CrudGen\CrudGen\GridViewTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_itemClass));
            
            #line default
            #line hidden
            this.Write("Grid\", columns);\r\n        client.Sortable(true).Filterable(true).Crud(true, CrudS" +
                    "ervice);\r\n        _grid = client.Grid;\r\n\r\n        // Set new items to grid\r\n    " +
                    "    _task = client.UpdateGrid();\r\n        await _task;\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class GridViewTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
