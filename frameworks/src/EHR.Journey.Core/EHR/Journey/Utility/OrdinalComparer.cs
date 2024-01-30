using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.Utility
{
    /// <summary>
    /// ASCII值排序
    /// </summary>
    public class OrdinalComparer : System.Collections.Generic.IComparer<String>
    {
        public int Compare(String x, String y)
        {
            return string.CompareOrdinal(x, y);
        }
    }
}
