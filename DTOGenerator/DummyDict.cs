using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTOGenerator
{
    class DummyDict : Dictionary<string, Dictionary<int, string>>
    {
        public DummyDict()
        {
            this[typeof(int).Name] = new Dictionary<int, string>();
            this[typeof(int).Name][0] = "1";
            this[typeof(int).Name][1] = "2";
            this[typeof(string).Name] = new Dictionary<int, string>();
            this[typeof(string).Name][0] = "\"a\"";
            this[typeof(string).Name][1] = "\"b\"";
            this[typeof(bool).Name] = new Dictionary<int, string>();
            this[typeof(bool).Name][0] = "true";
            this[typeof(bool).Name][1] = "false";
            this[typeof(DateTime).Name] = new Dictionary<int, string>();
            this[typeof(DateTime).Name][0] = "new DateTime(2012,1,1)";
            this[typeof(DateTime).Name][1] = "new DateTime(2012,2,1)";
        }

        public string GetParameter(PropertyInfo p, int count)
        {
            return this[p.Type][count];
        }
    }
}
