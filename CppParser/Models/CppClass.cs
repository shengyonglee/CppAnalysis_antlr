
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CppParser.Enums;

namespace CppParser.Models
{
    public class CppClass : CppElement
    {
        /// <summary>
        /// class类型，默认为"class"。包括"class对应类图中的类"、"struct对应类图中的类"、"interface对应类图中的接口"
        /// </summary>
        public EnumClassType Stereotype { get; set; } = EnumClassType.Class;

        /// <summary>
        /// 类的属性、组合属性、聚合属性、关联属性
        /// </summary>
        public List<CppProperty> Properties { get; set; } = new List<CppProperty>();

        /// <summary>
        /// 类的方法
        /// </summary>
        public List<CppMethod> Methods { get; set; } = new List<CppMethod>();

        /// <summary>
        /// 类内的枚举类型
        /// </summary>
        public List<CppEnum> Enums { get; set; } = new List<CppEnum>();

        /// <summary>
        /// 基类列表
        /// </summary>
        ///public List<string> BaseClasses { get; set; } = new List<string>();

        /// <summary>
        /// 继承的父类列表
        /// </summary>
        public List<CppGeneralization> Generalizations { get; set; } = new List<CppGeneralization>();
        /// <summary>
        /// 实现的接口列表
        /// </summary>
        public List<CppRealization> Realizations { get; set; } = new List<CppRealization>();

        /// <summary>
        /// 类的依赖关系
        /// </summary>
        public List<CppDependency> Dependencies { get; set; } = new List<CppDependency>();

        /// <summary>
        /// 类的关联关系
        /// </summary>
        public List<CppAssociation> Associations { get; set; } = new List<CppAssociation>();

        /// <summary>
        /// 类的组合关系
        /// </summary>
        public List<CppComposition> Compositions { get; set; } = new List<CppComposition>();

        /// <summary>
        /// 类的聚合关系
        /// </summary>
        public List<CppAggregation> Aggregations { get; set; } = new List<CppAggregation>();

        /// <summary>
        /// 是否为抽象类
        /// </summary>
        public bool IsAbstract { get; set; } = false;

        /// <summary>
        /// 是否为接口
        /// </summary>
        public bool IsInterface { get; set; } = false;

        /// <summary>
        /// 分析类的方法，判断是否为接口
        /// 接口：只包含纯虚方法​​（没有实现），​​不包含有实现的虚方法​​，且​​没有属性​​
        /// </summary>
        public void AnalyzeAsInterface()
        {
            if (Methods.Count == 0) return;

            // 是否存在纯虚函数 (=0)
            bool hasPureVirtual = Methods.Any(m => m.IsPureVirtual);

            // 是否存在普通函数（非虚）
            bool hasNormalMethod = Methods.Any(m => !m.IsVirtual && !m.IsPureVirtual);

            // 是否存在虚函数但不是纯虚函数
            bool hasVirtual = Methods.Any(m => m.IsVirtual && !m.IsPureVirtual);

            // 是否存在成员变量
            bool hasDataMember = Properties.Count > 0;

            // 抽象类：只要有纯虚或虚函数
            IsAbstract = hasPureVirtual || hasVirtual;

            // 接口：必须全部是纯虚函数，且无数据成员、无普通方法
            IsInterface = hasPureVirtual && !hasVirtual && !hasNormalMethod && !hasDataMember;

            if (IsInterface)
                Stereotype = EnumClassType.Interface;
            else if (IsAbstract)
                Stereotype = EnumClassType.AbstractClass;
            else
                Stereotype = EnumClassType.Class;
        }


    }
}
