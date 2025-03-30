using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppGenerator.Services
{
    /// <summary>生成结果（只返回字符串，不做文件 IO）。</summary>
    public sealed class RenderResult
    {
        public string HeaderCode { get; set; } = string.Empty;
        public string SourceCode { get; set; } = string.Empty;
    }
}