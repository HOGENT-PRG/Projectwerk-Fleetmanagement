using System;
using System.Collections.Generic;
using System.Reflection;

namespace BusinessLaag.Model.Attributes
{
#nullable enable
    public sealed class TableMapAttribute : Attribute
    {
        public string? ColumnName { get; set; }
        public string? TableName { get; set; }

        public TableMapAttribute(string? colName)
        {
            if(colName != null) ColumnName = colName;
        }

        public TableMapAttribute(string? tableName, bool isTableName=true)
        {
            if (tableName != null) TableName = tableName;
        }
    }
#nullable disable
}
