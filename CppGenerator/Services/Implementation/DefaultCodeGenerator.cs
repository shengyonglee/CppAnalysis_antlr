using System;
using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>门面：预处理 → 渲染（不落盘）。</summary>
    public sealed class DefaultCodeGenerator : ICodeGenerator
    {
        private readonly ICppModelPreprocessor _pre;
        private readonly ICodeRenderer _renderer;

        public DefaultCodeGenerator(ICppModelPreprocessor pre, ICodeRenderer renderer)
        {
            _pre = pre ?? throw new ArgumentNullException(nameof(pre));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public RenderResult Generate(CppClass model)
        {
            var fixedModel = _pre.Process(model);
            return _renderer.Render(fixedModel);
        }
    }
}
