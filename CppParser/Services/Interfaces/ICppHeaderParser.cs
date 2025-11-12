using CppParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Services.Interfaces
{
    /// <summary>
    /// C++头文件解析器接口
    /// </summary>
    public interface ICppHeaderParser
    {
        /// <summary>
        /// 解析头文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        CodeHeaderFile ParseHeaderFile(string filePath);

        /// <summary>
        /// 解析头文件内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        CodeHeaderFile ParseHeaderContent(string content, string fileName = "unknown.h");
    }
}
