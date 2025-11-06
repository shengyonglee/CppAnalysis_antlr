using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppParser.Models;

using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>
    /// C++ 模型预处理器接口
    /// </summary>
    public interface ICppModelPreprocessor
    {
        /// <summary>
        /// 预处理类模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CodeClass ProcessClass(CodeClass model);

        /// <summary>
        /// 预处理枚举模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CodeEnum ProcessEnum(CodeEnum model);
    }
}
