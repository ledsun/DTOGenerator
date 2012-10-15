using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTOGenerator
{
    /// <summary>
    /// プロパティ情報です。
    /// </summary>
    public class PropertyInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool AllowNull { get; set; }
        public bool IsID { get; set; }
        public bool IsSubTable { get; set; }

        private PropertyInfo(string name, string type, bool allowNull, bool isId, bool isSubTable)
        {
            Name = name;
            Type = name = type;
            AllowNull = allowNull;
            IsID = isId;
            IsSubTable = isSubTable;
        }

        //カラム用
        public PropertyInfo(string name, Type type, bool allowNull, bool isId)
            : this(
            name,
            name == "PatientId" ? "PatientId"
                : allowNull ? type.Name + "?"
                : type.Name,
            allowNull,
            isId,
            false) { }

        //サブテーブル用
        public PropertyInfo(string name, string type)
            : this(name, type, false, false, true) { }

        /// <summary>
        /// 変数定義
        /// </summary>
        public string VariableDeclare
        {
            get { return Type + " " + Name; }
        }

        /// <summary>
        /// 引数定義
        /// </summary>
        public string ParameterDeclare
        {
            get
            {
                return Type + " " + ParameterName
                    + (AllowNull ? " = null" : "");
            }
        }

        /// <summary>
        /// 頭が小文字の引数形式の名前
        /// </summary>
        public string ParameterName
        {
            get { return Name.Substring(0, 1).ToLower() + Name.Substring(1); }
        }

        /// <summary>
        /// コンストラクター内のプロパティへの代入
        /// </summary>
        public string AssigneProperty
        {
            get { return Name + " = " + ParameterName + ";"; }
        }
    }
}
