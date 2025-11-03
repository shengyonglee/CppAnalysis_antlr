using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Models
{
    public class CppMethodParameter : CppProperty
    {
        /// <summary>
        /// 参数传递方向，inout.in out。默认inout
        /// </summary>
        public string direction { get; set; } = "inout";
    }
}
