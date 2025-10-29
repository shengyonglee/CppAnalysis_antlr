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
        /// 方法参数是否为右值引用
        /// </summary>
        public bool IsRValueReference { get; set; }
    }
}
