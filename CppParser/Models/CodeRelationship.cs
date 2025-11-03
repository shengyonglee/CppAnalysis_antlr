using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CppParser.Enums;


namespace CppParser.Models
{
    public abstract class CodeRelationship
    {
        /// <summary>
        /// 目标名（class、struct、interface类名）
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 目标类型
        /// </summary>
        public EnumClassType TargetEnum { get; set; }

        /// <summary>
        /// 多重性
        /// </summary>
        public EnumCppMultiplicity Multiplicity { get; set; }

        /// <summary>
        /// 多重性固定大小（如有的话）
        /// </summary>
        public int? FixedSize { get; set; } 

        /// <summary>
        /// 成员名
        /// </summary>
        public string RoleName { get; set; }          
    }

    /// <summary>
    /// 泛化（继承）
    /// </summary>
    public class CodeGeneralization : CodeRelationship { } 

    /// <summary>
    /// 实现
    /// </summary>
    public class CodeRealization : CodeRelationship { }

    /// <summary>
    /// 依赖
    /// </summary>
    public class CodeDependency : CodeRelationship { }

    /// 使用关系

    /// <summary>
    /// 关联
    /// </summary>
    public class CodeAssociation : CodeRelationship 
    {
        /// <summary>
        /// 可见性
        /// <summary>
        public EnumVisibility Visibility { get; set; } = EnumVisibility.Public;
    }

    /// <summary>
    /// 组合
    /// </summary>
    public class CodeComposition : CodeRelationship { }

    /// <summary>
    /// 聚合
    /// </summary>
    public class CodeAggregation : CodeRelationship { }   
    
}
