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

        public int CountID
        {
            get
            {
                return Properties
                    .Where(p => p.IsID)
                    .Count();
            }
        }

        public string GetDummyParameters(int count)
        {
            return string.Join(
                ", ",
                Properties
                    .Where(p => p.IsID)
                    .Take(count)
                    .Select(p => p.ParameterDeclare)
            );
        }

        public string GetDummyConstructorParameters(int count)
        {
            return string.Join(
                ", ",
                Properties.Select(p =>
                {
                    return p.Name == "PatientId" ? "new PatientId(patientId)"
                        : (count > 0 && p.Name == "QuestionId") ? "questionId"
                        : p.GetDummyConstructorParameter(count);
                })
            );
        }
    }
}
