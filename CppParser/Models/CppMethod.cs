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
        /// 方法返回类型
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// 方法返回类型是否为指针
        /// </summary>
        public bool ReturnTypeIsPointer { get; set; }

        /// <summary>
        /// 方法返回类型是否为引用
        /// </summary>
        public bool ReturnTypeIsReference { get; set; }

        /// <summary>
        /// 方法返回类型是否为const
        /// </summary>
        public bool IsReturnConst { get; set; }

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

        /// <summary>
        /// 方法是否为显式构造函数
        /// </summary>
        public bool IsExplicit { get; set; }

        /// <summary>
        /// 方法是否为内联函数
        /// </summary>
        public bool IsInline { get; set; }

        /// <summary>
        /// 方法是否为友元函数
        /// </summary>
        public bool IsFriend { get; set; }

        /// <summary>
        /// 方法是否为constexpr函数
        /// </summary>
        public bool IsConstexpr { get; set; }

        /// <summary>
        /// 方法是否为const成员函数
        /// </summary>
        public bool IsConst { get; set; }

        /// <summary>
        /// 方法是否 = default 实现
        /// </summary>
        public bool IsDefaultImplementation { get; set; }

        /// <summary>
        /// 方法是否 = delete 删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 方法是否为 override 函数
        /// </summary>
        public bool IsOverride { get; set; }

        /// <summary>
        /// 方法是否为 final 函数
        /// </summary>
        public bool IsFinal { get; set; }
    }
}
