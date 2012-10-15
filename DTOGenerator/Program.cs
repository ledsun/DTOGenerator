using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Alhambra.Db.Helper;

namespace DTOGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Genelator.Genelate("T_QUESTION", new[] { "T_QUESTION_ABOUT", "T_QUESTION_PREGNANCY" });
        }
    }

    static class Genelator
    {
        public static void Genelate(string tableName, IEnumerable<string> subTableName)
        {
            var classData = GetClassInfoFromShcema(tableName);

            foreach (var t in subTableName)
            {
                Genelate(t, new string[] { });
                classData.AddSubTable(t);
            }

            classData.SortPropeties();
            var content = new ClassTemplate(classData).TransformText();
            System.IO.File.WriteAllText(classData.ClassName + ".cs", content);
        }

        private static ClassInfo GetClassInfoFromShcema(string tableName)
        {
            var classData = new ClassInfo();
            classData.ClassName = tableName.TableNameToPropertyName();

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
