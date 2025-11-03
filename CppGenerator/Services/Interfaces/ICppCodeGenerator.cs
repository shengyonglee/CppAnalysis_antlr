using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>门面：预处理 → 渲染；仅返回字符串。</summary>
    public interface ICppCodeGenerator
    {
        RenderResult GenerateClass(CodeClass model);
        string GenerateEnum(CodeEnum model);
        string GenerateInterface(CodeClass model);
    }
}
