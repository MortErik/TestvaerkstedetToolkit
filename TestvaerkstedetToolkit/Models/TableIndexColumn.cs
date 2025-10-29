using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestvaerkstedetToolkit.Models
{
    /// <summary>
    /// TableIndex kolonne med fuld metadata
    /// </summary>
    public class TableIndexColumn
    {
        public string Name { get; set; }
        public string ColumnID { get; set; }
        public string DataType { get; set; }        // XML datatype: VARCHAR, INTEGER, DECIMAL
        public string TypeOriginal { get; set; }    // Original format specifier
        public bool IsNullable { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }

        /// <summary>
        /// Display med alle metadata
        /// </summary>
        public string DisplayText => $"{ColumnID}: {Name} ({DataType}, {(IsNullable ? "nullable" : "not null")})";

        /// <summary>
        /// Get default værdi baseret på datatype
        /// </summary>
        public string GetDefaultValue()
        {
            if (IsNullable) return null; // Will generate xsi:nil="true"

            switch (DataType?.ToUpper())
            {
                case "DATE":
                    return "9999-12-31";
                case "TIME":
                    return "23:59:59";
                case "TIMESTAMP":
                    return "9999-12-31T23:59:59";
                case "INTEGER":
                    return "0";
                case "DECIMAL":
                    return "0.0";
                case "BOOLEAN":
                    return "false";
                default:
                    return "";
            }
        }
    }
}
