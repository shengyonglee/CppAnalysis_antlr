using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scriban;

namespace CppGenerator.Services
{
    /// <summary>
    /// C++ 模板提供者接口
    /// </summary>
    public interface ICppTemplateProvider
    {
        /// <summary>
        /// 获取类头文件模板
        /// </summary>
        /// <returns></returns>
        Template GetClassHeaderTemplate();

        /// <summary>
        /// 获取类源文件模板
        /// </summary>
        /// <returns></returns>
        Template GetClassSourceTemplate();

        /// <summary>
        /// 获取枚举头文件模板
        /// </summary>
        /// <returns></returns>
        Template GetEnumHeaderTemplate();

        /// <summary>
        /// 获取接口头文件模板
        /// </summary>
        /// <returns></returns>
        Template GetInterfaceHeaderTemplate();

        /// <summary>
        /// 获取结构体头文件模板
        /// </summary>
        /// <returns></returns>
        Template GetStructHeaderTemplate();
    }
}