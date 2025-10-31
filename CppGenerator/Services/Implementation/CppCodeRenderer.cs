using Scriban;
using Scriban.Runtime;
using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>用 Scriban 渲染类/枚举/接口，返回字符串。</summary>
    public sealed class CppCodeRenderer : ICppCodeRenderer
    {
        private readonly ICppTemplateProvider _provider;
        public CppCodeRenderer(ICppTemplateProvider provider) => _provider = provider;

        public RenderResult RenderClass(CppClass model)
        {
            var tctx = CreateContextForClass(model);
            var header = _provider.GetClassHeaderTemplate().Render(tctx);
            var source = _provider.GetClassSourceTemplate().Render(tctx);
            return new RenderResult { HeaderCode = header, SourceCode = source };
        }

        public string RenderEnum(CppEnum model)
        {
            var tctx = CreateContextForEnum(model);
            return _provider.GetEnumHeaderTemplate().Render(tctx);
        }

        public string RenderInterface(CppClass model)
        {
            var tctx = CreateContextForClass(model);
            return _provider.GetInterfaceHeaderTemplate().Render(tctx);
        }

        // ===== helpers =====
        private static TemplateContext CreateContextForClass(CppClass c)
        {
            var tctx = new TemplateContext { MemberRenamer = m => m.Name };
            var g = new ScriptObject();
            g.SetValue("c", c, true); 
            bool hasProtectedSection =
            bool hasPrivateSection = 

            tctx.PushGlobal(g);
            return tctx;
        }

        private static TemplateContext CreateContextForEnum(CppEnum e)
        {
            var tctx = new TemplateContext { MemberRenamer = m => m.Name };
            var g = new ScriptObject();
            g.SetValue("e", e, true);
            tctx.PushGlobal(g);
            return tctx;
        }
    }
}
