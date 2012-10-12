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
            var schema = GetShcema("T_QUESTION");
        }

        private static ClassData GetShcema(string tableName)
        {
            var classData = new ClassData();
            classData.ClassName = tableName
                .Substring(2)
                .SnakeCaseToCamelCase();

            var table = DBHelper.SelectTableSchema(tableName);
            foreach (DataColumn c in table.Columns)
            {
                classData.Properties.Add(
                    new PropertyData(c.ColumnName.SnakeCaseToCamelCase(),
                    c.DataType,
                    c.AllowDBNull,
                    table.PrimaryKey.Contains(c)));
            }

            return classData;
        }
    }

    public static class StringExtentions
    {
        public static string SnakeCaseToCamelCase(this string input)
        {
            if (Regex.IsMatch(input, "^[A-Z]{1}[0-9a-zA-Z]*$"))
            {
                return input;
            }
            else
            {
                return string.Join(
                    "",
                    input
                        .Split('_')
                        .Select(m => m.Substring(0, 1) + m.Substring(1).ToLower())
              );
            }
        }

        /// <summary>
        /// キャメルケース（大文字区切り）をスネークケース（大文字アンダーバー区切り）に変換します
        /// すでにスネークケースになっている文字列は変更しません。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CamelCaseToSnakeCase(this string input)
        {
            if (Regex.IsMatch(input, "^[0-9A-Z_]*$"))
            {
                return input;
            }
            else
            {
                return Regex.Replace(input, ".[A-Z]", m => m.ToString()[0] + "_" + m.ToString()[1]).ToUpper();
            }
        }
    }
}
