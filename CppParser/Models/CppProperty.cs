using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CppParser.Models
{
    public class CppProperty : CppElement
    {
        /// <summary>
        /// 属性类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 完整属性类型
        /// </summary>
        public string FullType { get; set; } // 包含所有修饰符的完整类型

        /// <summary>
        /// 是否为静态属性
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否为const属性
        /// </summary>
        public bool IsConst { get; set; }

        /// <summary>
        /// 是否为volatile属性
        /// </summary>
        public bool IsVolatile { get; set; }

        /// <summary>
        /// 是否为可变属性
        /// </summary>
        public bool IsMutable { get; set; }

        /// <summary>
        /// 是否为有符号类型
        /// </summary>
        public bool IsSigned { get; set; }

        /// <summary>
        /// 是否为无符号类型
        /// </summary>
        public bool IsUnsigned { get; set; }

        /// <summary>
        /// 是否为短整型
        /// </summary>
        public bool IsShort { get; set; }

        /// <summary>
        /// 是否为长整型
        /// </summary>
        public bool IsLong { get; set; }

        /// <summary>
        /// 是否为指针类型
        /// </summary>
        public bool IsPointer { get; set; }

        /// <summary>
        /// 是否为引用类型
        /// </summary>
        public bool IsReference { get; set; }

        /// <summary>
        /// 是否为数组类型
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// 数组大小
        /// </summary>
        public string ArraySize { get; set; } // 数组大小
    }
}
