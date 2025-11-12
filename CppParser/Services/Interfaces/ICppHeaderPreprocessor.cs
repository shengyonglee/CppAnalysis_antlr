using CppParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Services.Interfaces
{
    /// <summary>
    /// 头文件模型预处理器接口
    /// </summary>
    public interface ICppHeaderPreprocessor
    {
        /// <summary>
        /// 对CodeHeaderFile进行预处理
        /// </summary>
        CodeHeaderFile Preprocess(CodeHeaderFile headerFile);
    }
}
