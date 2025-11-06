using System;
using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>
    /// C++ 代码生成器实现
    /// </summary>
    public sealed class CppCodeGenerator : ICppCodeGenerator
    {
        private readonly ICppModelPreprocessor _pre;
        private readonly ICppCodeRenderer _renderer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="renderer"></param>
        public CppCodeGenerator(ICppModelPreprocessor pre, ICppCodeRenderer renderer)
        {
            _pre = pre ?? throw new ArgumentNullException(nameof(pre));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public RenderResult GenerateClass(CodeClass model)
        {
            var fixedModel = _pre.ProcessClass(model);
            return _renderer.RenderClass(fixedModel);
        }

        public string GenerateEnum(CodeEnum model)
        {
            var fixedModel = _pre.ProcessEnum(model);
            return _renderer.RenderEnum(fixedModel);
        }

        public string GenerateInterface(CodeClass model)
        {
            var fixedModel = _pre.ProcessClass(model);
            return _renderer.RenderInterface(fixedModel);
        }

        public string GenerateStruct(CodeClass model)
        {
            var fixedModel = _pre.ProcessClass(model);
            return _renderer.RenderStruct(fixedModel);
        }
    }
}
