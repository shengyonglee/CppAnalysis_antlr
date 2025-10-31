using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppParser.Models;


namespace CppGenerator.Services
{
    /// <summary>将模型与模板绑定并渲染为字符串。</summary>
    public interface ICppCodeRenderer
    {
        RenderResult RenderClass(CppClass model);   // .h + .cpp
        string RenderEnum(CppEnum model);   // 仅 .h
        string RenderInterface(CppClass model); // 仅 .h
    }
}
