using System;
using System.IO;
using Scriban;

namespace CppGenerator.Services
{
    /// <summary>从文件系统加载并编译 Scriban 模板。</summary>
    public sealed class CppTemplateProvider : ICppTemplateProvider
    {
        private readonly string _classHeaderPath;
        private readonly string _classSourcePath;
        private readonly string _enumHeaderPath;
        private readonly string _interfaceHeaderPath;
        private readonly string _structHeaderPath;

        private Template? _classHeaderTpl;
        private Template? _classSourceTpl;
        private Template? _enumHeaderTpl;
        private Template? _interfaceHeaderTpl;
        private Template? _structfaceHeaderTpl;

        /// <summary>
        /// 构造函数.classHeaderTemplatePath、interfaceHeaderTemplatePath和structHeaderTemplatePath都使用同一个模板class_header.sbn
        /// </summary>
        /// <param name="classHeaderTemplatePath"></param>
        /// <param name="classSourceTemplatePath"></param>
        /// <param name="enumHeaderTemplatePath"></param>
        /// <param name="interfaceHeaderTemplatePath"></param>
        /// <param name="structHeaderTemplatePath"></param>
        public CppTemplateProvider(
            string classHeaderTemplatePath,
            string classSourceTemplatePath,
            string enumHeaderTemplatePath,
            string interfaceHeaderTemplatePath,
            string structHeaderTemplatePath
            )
        {
            _classHeaderPath = classHeaderTemplatePath ?? throw new ArgumentNullException(nameof(classHeaderTemplatePath));
            _classSourcePath = classSourceTemplatePath ?? throw new ArgumentNullException(nameof(classSourceTemplatePath));
            _enumHeaderPath = enumHeaderTemplatePath ?? throw new ArgumentNullException(nameof(enumHeaderTemplatePath));
            _interfaceHeaderPath = interfaceHeaderTemplatePath ?? throw new ArgumentNullException(nameof(interfaceHeaderTemplatePath));
            _structHeaderPath = structHeaderTemplatePath ?? throw new ArgumentNullException(nameof(structHeaderTemplatePath));
        }

        public Template GetClassHeaderTemplate() => _classHeaderTpl ??= Compile(_classHeaderPath);
        public Template GetClassSourceTemplate() => _classSourceTpl ??= Compile(_classSourcePath);
        public Template GetEnumHeaderTemplate() => _enumHeaderTpl ??= Compile(_enumHeaderPath);
        public Template GetInterfaceHeaderTemplate() => _interfaceHeaderTpl ??= Compile(_interfaceHeaderPath);
        public Template GetStructHeaderTemplate() => _structfaceHeaderTpl ??= Compile(_structHeaderPath);

        /// <summary>
        /// 编译指定路径的模板
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static Template Compile(string path)
        {
            var text = File.ReadAllText(path);
            var tpl = Template.Parse(text, path);
            if (tpl.HasErrors)
                throw new InvalidOperationException($"Template parse error ({path}): {string.Join(Environment.NewLine, tpl.Messages)}");
            return tpl;
        }
    }
}
