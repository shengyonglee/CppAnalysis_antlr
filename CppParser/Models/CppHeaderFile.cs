using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppParser.Models
{
    public class CppHeaderFile
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件中的类列表
        /// </summary>
        public List<CppClass> Classes { get; set; } = new List<CppClass>();

        /// <summary>
        /// 文件中的枚举列表
        /// </summary>
        public List<CppEnum> Enums { get; set; } = new List<CppEnum>();
        // public List<string> Includes { get; set; } = new List<string>();
    }
}
