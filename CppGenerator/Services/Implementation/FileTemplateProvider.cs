using System;
using System.IO;
using Scriban;

namespace CppGenerator.Services
{
    /// <summary>从文件系统加载并编译 Scriban 模板。</summary>
    public sealed class FileTemplateProvider : ITemplateProvider
    {
        private readonly string _headerPath;
        private readonly string _sourcePath;
        private Template? _header;
        private Template? _source;

        public FileTemplateProvider(string headerTemplatePath, string sourceTemplatePath)
        {
            _headerPath = headerTemplatePath ?? throw new ArgumentNullException(nameof(headerTemplatePath));
            _sourcePath = sourceTemplatePath ?? throw new ArgumentNullException(nameof(sourceTemplatePath));
        }

        public Template GetHeaderTemplate() => _header ??= Compile(_headerPath);
        public Template GetSourceTemplate() => _source ??= Compile(_sourcePath);

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
