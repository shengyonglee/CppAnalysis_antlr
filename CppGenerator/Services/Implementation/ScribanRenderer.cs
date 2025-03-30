using Scriban;
using Scriban.Runtime;
using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>用 Scriban 渲染头/源文件，返回字符串。</summary>
    public sealed class ScribanRenderer : ICodeRenderer
    {
        private readonly ITemplateProvider _provider;
        public ScribanRenderer(ITemplateProvider provider) => _provider = provider;

        public RenderResult Render(CppClass model)
        {
            var tctx = new TemplateContext
            {
                // 不要自动改成员命名（默认会变 snake/camel）
                MemberRenamer = m => m.Name
            };

            // 与你现有模板变量保持一致
            var globals = new ScriptObject();
            globals.SetValue("c", model, true);
            globals.SetValue("class", model, true);


            tctx.PushGlobal(globals);

            var header = _provider.GetHeaderTemplate().Render(tctx);
            var source = _provider.GetSourceTemplate().Render(tctx);

            return new RenderResult { HeaderCode = header, SourceCode = source };
        }
    }
}
