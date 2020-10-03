using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen.Runtime
{
    /// <summary>
    /// Validation handler for Grid events (BeforeInserting, BeforeUpdating.)
    /// </summary>
    public class ValidationArgs
    {
        /// <summary>
        ///  general large error
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        ///  individual column errors (nameof(X))
        /// </summary>
        public Dictionary<string, string> ColumnErrors { get; set; }

        /// <summary>
        /// True if creating, otherwise false (updating)
        /// </summary>
        public bool IsNew { get; set; }

        public ValidationArgs(bool isNew, Dictionary<string, string> dict)
        {
            IsNew = isNew;
            ColumnErrors = dict;
        }
    }
}
