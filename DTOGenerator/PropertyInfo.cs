using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTOGenerator
{
    public class PropertyInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool AllowNull { get; set; }
        public bool IsID { get; set; }

        public PropertyInfo(string name, Type type, bool allowNull, bool isId)
        {
            Name = name;
            Type = name == "PatientId" ? "PatientId" : type.Name;
            AllowNull = allowNull;
            IsID = isId;
        }

        public string Declare
        {
            get { return Type + " " + Name; }
        }

        public string ParameterDeclare
        {
            get { return Type + " " + ParameterName; }
        }

        private string ParameterName
        {
            get { return Name.Substring(0, 1).ToLower() + Name.Substring(1); }
        }

        public string ConstructorProcess
        {
            get { return Name + " = " + ParameterName + ";"; }
        }

        public string GetDummyConstructorParameter(int count)
        {
            return DummyDict[Type][count];
        }

        private static DummyDict DummyDict = new DummyDict();
    }
}
