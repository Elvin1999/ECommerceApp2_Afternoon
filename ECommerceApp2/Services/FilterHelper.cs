using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.Services
{
    public class FilterModel
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
    }
    public class FilterHelper
    {

        //id=10
        //username = Elvin123
        //[username , Elvin123]
        public static FilterModel? GetFilterValues(string filter)
        {
            var operators = new[] { ">", "<", ">=", "<=", "=", "!=", "<>" };
            var foundOperator = operators.FirstOrDefault(o => filter.IndexOf(o) >=0);

            if (foundOperator != null)
            {
                var paramItems = filter.Split(foundOperator);
                if (paramItems.Length == 2)
                {
                    var property = paramItems[0].Trim();
                    var value = paramItems[1].Trim();

                    return new FilterModel
                    {
                        Property = property,
                        Value=value,
                        Operator=foundOperator
                    };
                }

            }
            return null;
        }
    }
}
