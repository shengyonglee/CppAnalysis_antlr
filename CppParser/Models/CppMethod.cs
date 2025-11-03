using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Models
{
    public class CppMethod : CppElement
    {

        /// <summary>
        /// 方法返回类型(void+基础类型)
        /// </summary>
        public string ReturnType { get; set; }


        /// <summary>
        /// 方法返回类型（自定义类型、枚举类型、数据类型） ，
        /// </summary>
        public string CustomReturnType { get; set; }


        /// <summary>
        /// 方法参数列表
        /// </summary>
        public List<CppMethodParameter> Parameters { get; set; } = new List<CppMethodParameter>();

        /// <summary>
        /// 方法是否为虚函数
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// 方法是否为纯虚函数
        /// </summary>
        public bool IsPureVirtual { get; set; }

        /// <summary>
        /// 方法是否为静态函数
        /// </summary>
        public bool IsStatic { get; set; }

    }
}
