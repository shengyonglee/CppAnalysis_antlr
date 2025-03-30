using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>将 CppClass 与模板绑定并渲染为字符串。</summary>
    public interface ICodeRenderer
    {
        RenderResult Render(CppClass model);
    }
}