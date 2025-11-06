using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CppParser.Models;


namespace CppGenerator.Services
{
    /// <summary>
    /// C++ 代码渲染器接口
    /// </summary>
    public interface ICppCodeRenderer
    {
        /// <summary>
        /// 渲染类代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        RenderResult RenderClass(CodeClass model);

        /// <summary>
        /// 渲染枚举代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string RenderEnum(CodeEnum model);

        /// <summary>
        /// 渲染接口代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string RenderInterface(CodeClass model);

        /// <summary>
        /// 渲染结构体代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string RenderStruct(CodeClass model); 
    }
}
