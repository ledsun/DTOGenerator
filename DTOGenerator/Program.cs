using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace DTOGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var classData = GetClassInfoFromShcema("T_QUESTION_ABOUT");
            var content = new ClassTemplate(classData).TransformText();
            System.IO.File.WriteAllText(classData.ClassName + ".cs", content);
        }

        private static ClassInfo GetClassInfoFromShcema(string tableName)
        {
            var classData = new ClassInfo();
            classData.ClassName = tableName
                .Substring(2)
                .SnakeCaseToCamelCase();

            var table = DBHelper.SelectTableSchema(tableName);
            foreach (DataColumn c in table.Columns)
            {
                if (c.ColumnName != "UPDATE_DATE" && c.ColumnName != "UPDATE_USER")
                {
                    classData.Properties.Add(
                        new PropertyInfo(c.ColumnName.SnakeCaseToCamelCase(),
                        c.DataType,
                        c.AllowDBNull,
                        table.PrimaryKey.Contains(c)));
                }
            }

            return classData;
        }
    }
}
