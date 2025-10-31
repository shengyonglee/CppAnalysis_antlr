using System;
using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>门面：预处理 → 渲染（类/枚举/接口）。</summary>
    public sealed class CppCodeGenerator : ICppCodeGenerator
    {
        private readonly ICppModelPreprocessor _pre;
        private readonly ICppCodeRenderer _renderer;

        public CppCodeGenerator(ICppModelPreprocessor pre, ICppCodeRenderer renderer)
        {
            _pre = pre ?? throw new ArgumentNullException(nameof(pre));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public RenderResult GenerateClass(CppClass model)
        {
            var fixedModel = _pre.ProcessClass(model);
            return _renderer.RenderClass(fixedModel);
        }

        public string GenerateEnum(CppEnum model)
        {
            var fixedModel = _pre.ProcessEnum(model);
            return _renderer.RenderEnum(fixedModel);
        }

        public string GenerateInterface(CppClass model)
        {
            // 接口仍复用 Class 的预处理（其中对 Interface 做了温和兜底）
            var fixedModel = _pre.ProcessClass(model);
            return _renderer.RenderInterface(fixedModel);
        }
    }
}
