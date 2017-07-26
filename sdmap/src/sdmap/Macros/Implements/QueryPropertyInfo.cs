using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.Macros.Implements
{
    public class QueryPropertyInfo
    {
        public string Name { get; }

        public Type PropertyType { get; }        

        public QueryPropertyInfo(string name, Type propertyType)
        {
            Name = name;
            PropertyType = propertyType;
        }
    }
}
