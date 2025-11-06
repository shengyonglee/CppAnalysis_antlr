using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppParser.Models;

namespace CppGenerator.Services
{
    /// <summary>
    /// C++ 代码生成器接口
    /// </summary>
    public interface ICppCodeGenerator
    {
        /// <summary>
        /// 生成类代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RenderResult GenerateClass(CodeClass model);

        /// <summary>
        /// 生成枚举代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string GenerateEnum(CodeEnum model);

        /// <summary>
        /// 生成接口代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string GenerateInterface(CodeClass model);

        /// <summary>
        /// 生成结构体代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string GenerateStruct(CodeClass model);
    }
}
