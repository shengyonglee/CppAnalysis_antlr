using System;
using System.Linq;
using CppParser.Enums;
using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>
    /// 轻量预处理：可见性兜底、排序、FixedSize 兜底、类型小修正等。
    /// 不改变业务语义，只做“生成友好化”。
    /// </summary>
    public sealed class DefaultModelPreprocessor : ICppModelPreprocessor
    {
        public CppClass Process(CppClass model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            // 1) 类名兜底
            model.Name = string.IsNullOrWhiteSpace(model.Name) ? "Unnamed" : model.Name.Trim();

            // 2) 可见性兜底
            foreach (var p in model.Properties ?? Enumerable.Empty<CppProperty>())
                if (p.Visibility == EnumVisibility.None) p.Visibility = EnumVisibility.Private;

            foreach (var m in model.Methods ?? Enumerable.Empty<CppMethod>())
                if (m.Visibility == EnumVisibility.None) m.Visibility = EnumVisibility.Public;

            // 3) Fixed 多重性兜底（没有 FixedSize -> 1）
            var allRels = model.Associations
                .Concat<CppRelationship>(model.Aggregations)
                .Concat(model.Compositions);
            foreach (var r in allRels)
            {
                if (r.Multiplicity == EnumCppMultiplicity.ToFixed && r.FixedSize == null)
                    r.FixedSize = 1;
            }

            // 4) 方法排序：构造 -> 析构 -> 非静态 -> 静态 -> 名字
            if (model.Methods != null)
            {
                model.Methods = model.Methods
                    .OrderBy(m => m.Name == model.Name ? 0 : (m.Name.StartsWith("~") ? 1 : 2))
                    .ThenBy(m => m.IsStatic ? 1 : 0)
                    .ThenBy(m => m.Name)
                    .ToList();
            }

            // 5）判断是否有protected成员,associate关系也需要考虑
            model.hasProtectedSection = (model.Properties != null && model.Properties.Any(p => p.Visibility == EnumVisibility.Protected))
                || (model.Methods != null && model.Methods.Any(m => m.Visibility == EnumVisibility.Protected)
                || (model.Associations != null && model.Associations.Any(x => x.Visibility == EnumVisibility.Protected)));

            // 6)  判断是否有private成员,associate关系也需要考虑
            model.hasPrivateSection = (model.Properties != null && model.Properties.Any(p => p.Visibility == EnumVisibility.Private))
                || (model.Methods != null && model.Methods.Any(m => m.Visibility == EnumVisibility.Private)
                || (model.Associations != null && model.Associations.Any(x => x.Visibility == EnumVisibility.Private)));

            return model;
        }
    }
}
