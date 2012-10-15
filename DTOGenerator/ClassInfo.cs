using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTOGenerator
{
    public class ClassInfo
    {
        public string ClassName { get; set; }
        public List<PropertyInfo> Properties { get; set; }

        public ClassInfo()
        {
            Properties = new List<PropertyInfo>();
        }

        /// <summary>
        /// コンストラクタの引数
        /// </summary>
        public string ConstructorPalameters
        {
            get
            {
                return string.Join(
                    ", ",
                    Properties.Select(p => p.ParameterDeclare)
                );
            }
        }

        /// <summary>
        /// Equalsメソッドの処理の中身
        /// </summary>
        public string EqualsProcess
        {
            get
            {
                return string.Join(
                    " && ",
                    Properties
                        .Where(p => !p.IsID)
                        .Select(p => p.Name + " == other." + p.Name)
                );
            }
        }

        /// <summary>
        /// GetHashCodeメソッドの処理の中身
        /// </summary>
        public string GetHashCodeProcess
        {
            get
            {
                return string.Join(
                    " ^ ",
                    Properties
                        .Where(p => !p.IsID)
                        .Select(p => p.Name + ".GetHashCode()")
                );
            }
        }

        /// <summary>
        /// IDの個数を返します。
        /// </summary>
        public int CountID
        {
            get
            {
                return IDs.Count();
            }
        }

        /// <summary>
        /// ダミーメソッドの引数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string GetDummyParameters(int count)
        {
            return string.Join(
                ", ",
                IDs
                    .Take(count)
                    .Select(p => p.ParameterDeclare)
            );
        }

        /// <summary>
        /// ダミーメソッド内の自コンストラクタの引数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string GetDummyConstructorParameters(int count)
        {
            return string.Join(
                ", ",
                Properties.Select(p =>
                {
                    return p.Name == "PatientId" ? "new PatientId(patientId)"
                        : (count > 0 && p.Name == "QuestionId") ? "questionId"
                        : p.IsSubTable ? string.Format("{0}.Dummy({1})", p.Name, GetSubClassDummyParameters(count))
                        : p.Type.EndsWith("?") ? "null"
                        : DummyDict.GetParameter(p, count);
                })
            );
        }

        /// <summary>
        /// サブテーブルを追加します。
        /// </summary>
        /// <param name="subTableName"></param>
        public void AddSubTable(string subTableName)
        {
            var name = subTableName.TableNameToPropertyName();
            Properties.Add(new PropertyInfo(name, name));
        }

        /// <summary>
        /// プロパティーを並べ替えます。
        /// null可のプロパティを後ろに下げて、デフォルト引数にできるようにします。
        /// </summary>
        internal void SortPropeties()
        {
            Properties = Properties
                .OrderBy(p => p.AllowNull)
                .ToList();
        }

        /// <summary>
        /// IDを返します。
        /// </summary>
        private IEnumerable<PropertyInfo> IDs
        {
            get { return Properties.Where(p => p.IsID); }
        }

        /// <summary>
        /// サブクラスのダミーメソッドの引数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private string GetSubClassDummyParameters(int count)
        {
            return string.Join(
                ", ",
                IDs
                    .Take(count + 1)
                    .Select(p => p.ParameterName)
            );
        }

        private static DummyDict DummyDict = new DummyDict();
    }
}
