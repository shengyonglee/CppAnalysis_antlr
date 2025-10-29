using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CppParser.Enums;


namespace CppParser.Models
{
    public abstract class CppRelationship
    {
        public string TargetClass { get; set; }       // 目标类名
        public EnumCppMultiplicity Multiplicity { get; set; }
        public string RoleName { get; set; }          // 成员名
    }

    public class CppGeneralization : CppRelationship { } // 泛化（继承）
    public class CppRealization : CppRelationship { }    // 实现（接口继承）
    public class CppDependency : CppRelationship { }     // 依赖
    public class CppAssociation : CppRelationship { }    // 关联
    public class CppComposition : CppRelationship { }    // 组合
    public class CppAggregation : CppRelationship { }    // 聚合
    
}
