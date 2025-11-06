using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppGenerator.Services
{
    /// <summary>
    /// 渲染结果
    /// </summary>
    public sealed class RenderResult
    {
        /// <summary>
        /// 头文件代码
        /// </summary>
        public string HeaderCode { get; set; } = string.Empty;

        /// <summary>
        /// 源文件代码
        /// </summary>
        public string SourceCode { get; set; } = string.Empty;
    }
}